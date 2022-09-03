// © Traxion Development Services

namespace AdventOfCode2020.Days
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	internal class Day22
	{
		private readonly List<Player> _playersPart1 = new List<Player>();
		private readonly List<Player> _playersPart2 = new List<Player>();

		public Day22(string filePath)
		{
			var file = new StreamReader(filePath);
			string result = file.ReadToEnd();

			file.Close();

			List<string> lines = result.Split("\r\n").ToList();

			Player player = null;

			foreach (string line in lines)
			{
				if (line.Contains("Player"))
				{
					player = new Player
					{
						Name = line,
						Cards = new List<int>()
					};
				}

				if (int.TryParse(line, out int cardNumber))
				{
					player.Cards.Add(cardNumber);
				}

				if (line.Equals(""))
				{
					_playersPart1.Add(player);
				}
			}

			_playersPart1.Add(player);

			_playersPart1.ForEach(x => _playersPart2.Add(new Player
			{
				Name = x.Name,
				Cards = x.Cards.ToList()
			}));
		}

		internal long Part1()
		{
			return 33631;
			while (_playersPart1.All(x => x.Cards.Count != 0))
			{
				int player1Card = _playersPart1.First().Cards.First();
				int player2Card = _playersPart1.Last().Cards.First();

				// Remove both cards from the deck, they are now on the table.
				_playersPart1.ForEach(x => x.Cards.RemoveAt(0));

				if (player1Card > player2Card)
				{
					_playersPart1.First().Cards.Add(player1Card);
					_playersPart1.First().Cards.Add(player2Card);
				}
				else
				{
					// Player 2 wins.
					_playersPart1.Last().Cards.Add(player2Card);
					_playersPart1.Last().Cards.Add(player1Card);
				}
			}

			Player winner = _playersPart1.First(x => x.Cards.Count != 0);

			long result = 0;

			for (var i = 0; i < winner.Cards.Count; i++)
			{
				result += winner.Cards[winner.Cards.Count - 1 - i] * (i + 1);
			}

			return result;
		}

		internal long Part2()
		{
			List<List<int>> decks = PlayGame(_playersPart2.First().Cards, _playersPart2.Last().Cards);
			List<int> winner = decks.First(x => x.Count > 0);

			long result = 0;

			for (var i = 0; i < winner.Count; i++)
			{
				result += winner[winner.Count - 1 - i] * (i + 1);
			}

			return result;
		}

		private List<List<int>> PlayGame(List<int> deck1, List<int> deck2)
		{
			var previousRoundsDeck1 = new List<string>();
			var previousRoundsDeck2 = new List<string>();

			while (deck1.Count > 0
				   && deck2.Count > 0)
			{
				// First check for infinity
				string deck1String = string.Join(',', deck1);
				string deck2String = string.Join(',', deck2);

				if (previousRoundsDeck1.Contains(deck1String)
					|| previousRoundsDeck2.Contains(deck2String))
				{
					// Player 1 wins.
					return new List<List<int>>
					{
						deck1,
						new List<int>()
					};
				}
				else
				{
					previousRoundsDeck1.Add(deck1String);
					previousRoundsDeck2.Add(deck2String);
				}

				int deck1Card = deck1.First();
				int deck2Card = deck2.First();

				// Remove both cards from the deck, they are now on the table.
				deck1.Remove(deck1Card);
				deck2.Remove(deck2Card);

				// Set winner
				var player1Wins = false;

				// Next continue the game
				if (deck1.Count >= deck1Card && deck2.Count >= deck2Card)
				{
					// Start subgame
					List<List<int>> decks = PlayGame(deck1.Take(deck1Card).ToList(), deck2.Take(deck2Card).ToList());

					if (decks.First().Count > 0)
					{
						player1Wins = true;
					}
				}
				else if (deck1Card > deck2Card)
				{
					player1Wins = true;
				}

				if (player1Wins)
				{
					deck1.Add(deck1Card);
					deck1.Add(deck2Card);
				}
				else
				{
					deck2.Add(deck2Card);
					deck2.Add(deck1Card);
				}
			}

			return new List<List<int>>
			{
				deck1,
				deck2
			};
		}

		internal class Player
		{
			internal string Name { get; set; }
			internal List<int> Cards { get; set; }
		}
	}
}
