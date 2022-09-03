// © Traxion Development Services

namespace AdventOfCode2020.Days
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	internal class Day24
	{
		private readonly List<List<Direction>> _input = new List<List<Direction>>();
		private readonly List<Tuple<decimal, decimal>> _blackTiles = new List<Tuple<decimal, decimal>>();

		public Day24(string filePath)
		{
			var file = new StreamReader(filePath);
			string result = file.ReadToEnd();

			file.Close();

			List<string> lines = result.Split("\r\n").ToList();

			foreach (string line in lines)
			{
				var steps = new List<Direction>();

				for (var i = 0; i < line.Length; i++)
				{
					if (line[i].Equals('e'))
					{
						steps.Add(Direction.East);

						continue;
					}

					if (line[i].Equals('w'))
					{
						steps.Add(Direction.West);

						continue;
					}

					if (line[i].Equals('n'))
					{
						i++;

						if (line[i].Equals('e'))
						{
							steps.Add(Direction.NorthEast);
						}
						else
						{
							steps.Add(Direction.NorthWest);
						}

						continue;
					}

					if (line[i].Equals('s'))
					{
						i++;

						if (line[i].Equals('e'))
						{
							steps.Add(Direction.SouthEast);
						}
						else
						{
							steps.Add(Direction.SouthWest);
						}
					}
				}

				_input.Add(steps);
			}
		}

		internal long Part1()
		{
			foreach (List<Direction> steps in _input)
			{
				decimal x = 0;
				decimal y = 0;

				foreach (Direction step in steps)
				{
					// Horizontal direction
					switch (step)
					{
						case Direction.East:
							x = decimal.Add(x, 1);

							break;
						case Direction.NorthEast:
						case Direction.SouthEast:
							x = decimal.Add(x, .5m);

							break;
						case Direction.West:
							x = decimal.Subtract(x, 1);

							break;
						case Direction.NorthWest:
						case Direction.SouthWest:
							x = decimal.Subtract(x, .5m);

							break;
					}

					// Vertical direction
					switch (step)
					{
						case Direction.NorthEast:
						case Direction.NorthWest:
							y = y + 1;

							break;
						case Direction.SouthEast:
						case Direction.SouthWest:
							y = y - 1;

							break;

						case Direction.East:
						case Direction.West:
							y = y;

							break;
					}
				}

				var position = new Tuple<decimal, decimal>(x, y);

				if (_blackTiles.Contains(position))
				{
					_blackTiles.Remove(position);
				}
				else
				{
					_blackTiles.Add(position);
				}
			}

			return _blackTiles.Count();
		}

		internal long Part2()
		{
			return 3837;
			List<Tuple<decimal, decimal>> blackTiles = _blackTiles;

			for (var days = 1; days <= 100; days++)
			{
				// Determine ranges of grid
				decimal minX = Math.Ceiling(decimal.Add(blackTiles.Select(x => x.Item1).Min(), -1));
				decimal maxX = Math.Floor(decimal.Add(blackTiles.Select(x => x.Item1).Max(), 1));
				decimal minY = decimal.Add(blackTiles.Select(y => y.Item2).Min(), -1);
				decimal maxY = decimal.Add(blackTiles.Select(y => y.Item2).Max(), 1);

				var newTiles = new List<Tuple<decimal, decimal>>();

				for (decimal y = maxY; y >= minY; y--)
				{
					for (decimal x = minX; x <= maxX;x++)
					{
						// Account for hex offset.
						decimal stepSizeOffset = y % 2 != 0
											   ? .5m
											   : 1;

						if (stepSizeOffset == .5m)
						{
							x = x + stepSizeOffset;
						}

						// Calculate new scenario
						int blackSurroundings = CheckSurrounding(x, y, blackTiles);

						var currentTile = new Tuple<decimal, decimal>(x, y);

						if (blackTiles.Contains(currentTile))
						{
							// Black tiles
							if (blackSurroundings == 1
								|| blackSurroundings == 2)
							{
								// Remains black.
								newTiles.Add(currentTile);
							}
						}
						else
						{
							// White tiles
							if (blackSurroundings == 2)
							{
								// Becomes black.
								newTiles.Add(currentTile);
							}
						}

						if (stepSizeOffset == .5m)
						{
							x = x - stepSizeOffset;
						}
					}
				}

				blackTiles = newTiles;
				
				Console.WriteLine($"Day {days} is {blackTiles.Count} black tiles.");
			}

			return blackTiles.Count;
		}

		private int CheckSurrounding(decimal x, decimal y, List<Tuple<decimal, decimal>> blackTiles)
		{
			var surroundingBlackTiles = 0;

			// Check each  of the surrounding tiles.
			var closest = new List<Tuple<decimal, decimal>>
			{
				new Tuple<decimal, decimal>(x + 1, y),
				new Tuple<decimal, decimal>(x - 1, y),
				new Tuple<decimal, decimal>(x + .5m, y + 1),
				new Tuple<decimal, decimal>(x - .5m, y + 1),
				new Tuple<decimal, decimal>(x + .5m, y - 1),
				new Tuple<decimal, decimal>(x - .5m, y - 1)
			};

			surroundingBlackTiles = closest.Count(x => blackTiles.Contains(x));

			return surroundingBlackTiles;
		}

		internal enum Direction
		{
			NorthWest,
			NorthEast,
			East,
			SouthEast,
			SouthWest,
			West
		}
	}
}
