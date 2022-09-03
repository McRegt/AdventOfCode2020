// © Traxion Development Services

namespace AdventOfCode2020.Days
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text.RegularExpressions;

	internal class Day20
	{
		private readonly List<Tile> _input = new List<Tile>();

		public Day20(string filePath)
		{
			var file = new StreamReader(filePath);
			string result = file.ReadToEnd();

			file.Close();

			List<string> lines = result.Split("\r\n").ToList();

			var id = 0;
			var content = new List<string>();

			// Read the tiles.
			foreach (string line in lines)
			{
				if (line.StartsWith("Tile"))
				{
					id = int.Parse(line.Substring(5, 4));
				}
				else if (line != string.Empty)
				{
					content.Add(line);
				}
				else
				{
					_input.Add(new Tile
					{
						Id = id,
						Content = content,
						Edges = new List<string>(),
						Matches = 0,
						MatchingTiles = new List<Tile>()
					});

					id = 0;
					content = new List<string>();
				}
			}

			// Determine the edges
			foreach (Tile tile in _input)
			{
				tile.Edges.Add(tile.Content.First());
				tile.Edges.Add(tile.Content.Last());

				var left = "";
				var right = "";

				tile.Content.ForEach(x => left += x.First());
				tile.Content.ForEach(x => right += x.Last());

				tile.Edges.Add(left);
				tile.Edges.Add(right);
			}
		}

		internal long Part1()
		{
			foreach (Tile tileToMatch in _input)
			{
				var matches = 0;
				Tile matchingTile = null;

				foreach (Tile tile in _input.Where(x => !x.Id.Equals(tileToMatch.Id)))
				{
					foreach (string edge in tile.Edges)
					{
						var reverseEdge = new string(edge.Reverse().ToArray());

						if (tileToMatch.Edges.Contains(edge)
							|| tileToMatch.Edges.Contains(reverseEdge))
						{
							matches++;
							tileToMatch.MatchingTiles.Add(tile);

							break;
						}
					}
				}

				tileToMatch.Matches = matches;
			}

			List<Tile> cornerTiles = _input.Where(x => x.Matches.Equals(2)).ToList();

			long startingNumber = 1;

			foreach (Tile cornerTile in cornerTiles)
			{
				startingNumber = startingNumber * cornerTile.Id;
			}

			return startingNumber;
		}

		internal long Part2()
		{
			List<string> image = BuildActualImage();

			long waveCount = 0;

			image.ForEach(x =>
			{
				char[] characters = x.ToCharArray();

				foreach (char character in characters)
				{
					if (character.Equals('#'))
					{
						waveCount++;
					}
				}
			});

			long monsterCount = 0;

			var headMonsterRegex = new Regex(@".{18}#.{1}");
			var mainMonsterRegex = new Regex(@"#.{4}##.{4}##.{4}###");
			var legsMonsterRegex = new Regex(@".{1}#.{2}#.{2}#.{2}#.{2}#.{2}#.{3}");

			var rotateCount = 1;

			while (monsterCount == 0)
			{
				for (var i = 1; i < image.Count - 1; i++)
				{
					foreach (Match match in mainMonsterRegex.Matches(image[i]))
					{
						string previousLine = image[i - 1].Substring(match.Index, match.Length);
						string nextLine = image[i + 1].Substring(match.Index, match.Length);

						if (headMonsterRegex.IsMatch(previousLine)
							&& legsMonsterRegex.IsMatch(nextLine))
						{
							monsterCount++;
						}
					}
				}

				if (monsterCount == 0)
				{
					// Rotate for more answers?
					if (rotateCount <= 4)
					{
						var rotated = new List<string>();

						for (int i = image[0].Length - 1; i >= 0; i--)
						{
							var newLine = "";

							foreach (string line in image)
							{
								newLine += line[i];
							}

							rotated.Add(newLine);
						}

						image = rotated;
						rotateCount++;
					}
					else
					{
						rotateCount = 1;

						var flipped = new List<string>();

						foreach (string content in image)
						{
							flipped.Add(new string(content.Reverse().ToArray()));
						}

						image = flipped;
					}
				}
			}

			long remainingWaves = waveCount - monsterCount * 15;

			return remainingWaves;
		}

		private List<string> BuildActualImage()
		{
			// Start on the top left corner
			Tile firstTile = _input.First(x => x.Matches.Equals(2));

			// Hard coded rotation for the first corner tile. :D
			//FlipHorizontal(firstTile); // Only relevant for sample input.
			RotateLeft(firstTile);
			RotateLeft(firstTile);

			// Build grid with correct rotations.
			var tileLines = new List<List<Tile>>
			{
				BuildEdge(firstTile)
			};

			while (_input.Count > 0)
			{
				tileLines.Add(BuildLine(tileLines.Last()));
			}

			// Remove all the edges.
			tileLines.ForEach(x => x.ForEach(RemoveEdge));

			// Combine into single large grid.
			var largeGrid = new List<string>();

			foreach (List<Tile> tileLine in tileLines)
			{
				for (var i = 0; i < tileLine[0].CleanContent.Count; i++)
				{
					var line = "";

					foreach (Tile tile in tileLine)
					{
						line += tile.CleanContent[i];
					}

					largeGrid.Add(line);
				}
			}

			return largeGrid;
		}

		private void FlipHorizontal(Tile tile)
		{
			var flipped = new List<string>();

			foreach (string content in tile.Content)
			{
				flipped.Add(new string(content.Reverse().ToArray()));
			}

			tile.Content = flipped;
		}

		private void RotateLeft(Tile tile)
		{
			var rotated = new List<string>();

			for (var i = 9; i >= 0; i--)
			{
				var newLine = "";

				foreach (string line in tile.Content)
				{
					newLine += line[i];
				}

				rotated.Add(newLine);
			}

			tile.Content = rotated;
		}

		private void RemoveEdge(Tile tile)
		{
			var newContent = new List<string>();

			for (var i = 0; i < tile.Content.Count; i++)
			{
				string line = tile.Content[i];

				// Skip first and last line
				if (i == 0
					|| i == 9)
				{
					continue;
				}

				// Otherwise, take substring.
				newContent.Add(line.Substring(1, line.Length - 2));
			}

			tile.CleanContent = newContent;
		}

		private List<Tile> BuildLine(List<Tile> previousLine)
		{
			var lineList = new List<Tile>();

			for (var i = 0; i < previousLine.Count; i++)
			{
				Tile previousTile = previousLine[i];

				List<Tile> tileList = _input.Where(x => x.MatchingTiles.Contains(previousTile)).ToList();
				previousLine.ForEach(x => tileList.Remove(x));

				if (i == 0)
				{
					FindFirstPosition(previousLine.First(), tileList.First());
				}
				else
				{
					FindPosition(lineList.Last(), tileList.First());
				}

				lineList.Add(tileList.First());

				_input.Remove(tileList.First());
			}

			return lineList;
		}

		private string GetLeftEdge(Tile tile)
		{
			var leftEdge = "";

			foreach (string line in tile.Content)
			{
				leftEdge += line.First();
			}

			return leftEdge;
		}

		private string GetRightEdge(Tile tile)
		{
			var rightEdge = "";

			foreach (string line in tile.Content)
			{
				rightEdge += line.Last();
			}

			return rightEdge;
		}

		private void FindFirstPosition(Tile lastTile, Tile currentTile)
		{
			string bottomEdge = lastTile.Content.Last();

			var rotateCount = 1;

			while (true)
			{
				string topEdge = currentTile.Content.First();

				if (bottomEdge.Equals(topEdge))
				{
					break;
				}

				// Rotate
				if (rotateCount <= 4)
				{
					rotateCount++;
					RotateLeft(currentTile);
				}
				else
				{
					rotateCount = 1;
					FlipHorizontal(currentTile);
				}
			}
		}

		private void FindPosition(Tile lastTile, Tile currentTile)
		{
			string rightEdge = GetRightEdge(lastTile);

			var rotateCount = 1;

			while (true)
			{
				string leftEdge = GetLeftEdge(currentTile);

				if (rightEdge.Equals(leftEdge))
				{
					break;
				}

				// Rotate
				if (rotateCount <= 4)
				{
					rotateCount++;
					RotateLeft(currentTile);
				}
				else
				{
					rotateCount = 1;
					FlipHorizontal(currentTile);
				}
			}
		}

		private List<Tile> BuildEdge(Tile corner)
		{
			var edgeList = new List<Tile>
			{
				corner
			};

			var cornerFound = false;

			while (!cornerFound)
			{
				foreach (Tile nextTile in edgeList.Last().MatchingTiles)
				{
					// Skip entry if already in the edge.
					if (edgeList.Contains(nextTile))
					{
						continue;
					}

					// Next corner found.
					if (nextTile.MatchingTiles.Count == 2)
					{
						FindPosition(edgeList.Last(), nextTile);
						edgeList.Add(nextTile);
						cornerFound = true;

						break;
					}

					// Next edge piece found.
					if (nextTile.MatchingTiles.Count == 3)
					{
						FindPosition(edgeList.Last(), nextTile);
						edgeList.Add(nextTile);

						break;
					}

					// Any other piece, is a center piece.
				}

				// Remove them from the input, they have been processed and are no longer relevant.
				foreach (Tile tile in edgeList)
				{
					_input.Remove(tile);
				}
			}

			return edgeList;
		}

		internal class Tile
		{
			internal int Id { get; set; }
			internal List<string> Content { get; set; }
			internal List<string> CleanContent { get; set; }
			internal List<string> Edges { get; set; }
			internal int Matches { get; set; }
			internal List<Tile> MatchingTiles { get; set; }
		}
	}
}
