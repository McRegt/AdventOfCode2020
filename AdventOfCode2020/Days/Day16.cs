// © Traxion Development Services

namespace AdventOfCode2020.Days
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Reflection.Metadata.Ecma335;
	using System.Text.RegularExpressions;

	internal class Day16
	{
		private readonly List<Rule> _rules = new List<Rule>();
		private readonly List<int> _myTicket;
		private readonly List<List<int>> _nearbyTickets = new List<List<int>>();
		private readonly List<List<int>> _validNearbyTickets = new List<List<int>>();

		public Day16(string filePath)
		{
			var file = new StreamReader(filePath);
			string result = file.ReadToEnd();

			file.Close();

			List<string> lines = result.Split("\r\n").ToList();

			var nextLineIsYourTicket = false;
			var nextLineAreOtherTickets = false;
			foreach (string line in lines)
			{
				if (line.Contains(" or "))
				{
					var rule = new Rule
					{
						Name = line.Split(':')[0],
						Ranges = new List<Tuple<int, int>>(),
						PossibleIndexes = new List<int>(),
						FinalIndex = -1
					};

					MatchCollection matches = new Regex(@"(\d+-\d+)").Matches(line);

					foreach (Match match in matches)
					{
						string[] values = match.Value.Split('-');
						rule.Ranges.Add(new Tuple<int, int>(int.Parse(values[0]), int.Parse(values[1])));
					}

					_rules.Add(rule);
				}

				if (line.Equals("your ticket:"))
				{
					nextLineIsYourTicket = true;

					continue;
				}

				if (line.Equals("nearby tickets:"))
				{
					nextLineAreOtherTickets = true;

					continue;
				}

				if (nextLineIsYourTicket)
				{
					_myTicket = line.Split(',').Select(int.Parse).ToList();
					nextLineIsYourTicket = false;

					continue;
				}

				if (nextLineAreOtherTickets)
				{
					_nearbyTickets.Add(line.Split(',').Select(int.Parse).ToList());
				}
			}
		}

		internal long Part1()
		{
			long errorValues = 0;

			foreach (List<int> nearbyTicket in _nearbyTickets)
			{
				var validTicket = true;

				foreach (int numberOnTicket in nearbyTicket)
				{
					var ruleApplies = false;
					foreach (Rule rule in _rules)
					{
						foreach (Tuple<int, int> range in rule.Ranges)
						{
							if (numberOnTicket >= range.Item1
								&& numberOnTicket <= range.Item2)
							{
								ruleApplies = true;

								break;
							}
						}

						if (ruleApplies)
						{
							break;
						}
					}

					if (!ruleApplies)
					{
						validTicket = false;
						errorValues += numberOnTicket;
					}
				}

				if (validTicket)
				{
					_validNearbyTickets.Add(nearbyTicket);
				}
			}

			return errorValues;
		}

		internal long Part2()
		{
			foreach (Rule rule in _rules)
			{
				var possibleIndexes = new List<int>();
				// Validate a given index of the ticket
				for (var i = 0; i < _validNearbyTickets[0].Count;i++)
				{

					var indexPossible = true;

					// For each ticket to see if it matches the rule.
					foreach (List<int> validTicket in _validNearbyTickets)
					{
						int value = validTicket[i];

						if (value >= rule.Ranges[0].Item1 && value <= rule.Ranges[0].Item2
							|| value >= rule.Ranges[1].Item1 && value <= rule.Ranges[1].Item2)
						{
							// Value is valid.
							continue;
						}

						indexPossible = false;

						break;
					}

					if (indexPossible)
					{
						possibleIndexes.Add(i);
					}
				}

				if (possibleIndexes.Count > 0)
				{
					rule.PossibleIndexes = possibleIndexes;
				}
			}

			// Process the rules, the possible indexes must be processed.
			while (_rules.Any(x => x.FinalIndex.Equals(-1)))
			{
				List<Rule> rulesWithOneOption = _rules.Where(x => x.PossibleIndexes.Count == 1).ToList();

				foreach (Rule rule in rulesWithOneOption)
				{
					rule.FinalIndex = rule.PossibleIndexes[0];

					// Update the other rules, by removing this value from the list of possibilities.
					foreach (Rule remainingRule in _rules.Where(x => x.PossibleIndexes.Contains(rule.FinalIndex)))
					{
						remainingRule.PossibleIndexes.Remove(rule.FinalIndex);
					}
				}
			}

			List<int> positions = _rules.Where(x => x.Name.StartsWith("departure")).Select(x => x.FinalIndex).ToList();

			long returnValue = _myTicket[positions[0]];

			for (var index = 1; index < positions.Count; index++)
			{
				int position = positions[index];
				returnValue = returnValue * _myTicket[position];
			}

			return returnValue;
		}

		internal class Rule
		{
			internal string Name { get; set; }
			internal List<Tuple<int, int>> Ranges { get; set; }
			internal List<int> PossibleIndexes { get; set; }
			internal int FinalIndex { get; set; }
		}
	}
}
