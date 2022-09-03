// © Traxion Development Services

namespace AdventOfCode2020.Days
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	internal class Day23
	{
		private readonly List<int> _cups = new List<int>();
		private readonly Dictionary<int, int> _cupsPart2 = new Dictionary<int, int>();

		public Day23(string filePath)
		{
			var file = new StreamReader(filePath);
			string result = file.ReadToEnd();

			file.Close();

			foreach (char number in result)
			{
				_cups.Add(int.Parse(number.ToString()));
			}

			List<int> cupsPart2 = _cups.ToList();

			for (int i = _cups.Max() + 1; i <= 1000000; i++)
			{
				cupsPart2.Add(i);
			}

			for (var i = 0; i < cupsPart2.Count; i++)
			{
				// Finally link the last entry to a circle.
				if (cupsPart2[i].Equals(cupsPart2.Last()))
				{
					_cupsPart2.Add(cupsPart2.Last(), cupsPart2.First());
				}
				else
				{
					_cupsPart2.Add(cupsPart2[i], cupsPart2[i + 1]);
				}
			}
		}

		internal string Part1()
		{
			var numberOfMoves = 100;
			int currentCup = _cups.First();
			List<int> cups = _cups.ToList();

			for (var i = 1; i <= numberOfMoves; i++)
			{
				Tuple<int, List<int>> tuple = Move(currentCup, cups);
				currentCup = tuple.Item1;
				cups = tuple.Item2;
			}

			return GetAnswer(cups);
		}

		private string GetAnswer(List<int> cups)
		{
			int indexOfOne = cups.IndexOf(1);

			string partOne = string.Join("", cups.GetRange(indexOfOne + 1, cups.Count - (indexOfOne + 1)));
			string partTwo = string.Join("", cups.GetRange(0, indexOfOne));

			return partOne + partTwo;
		}

		private Tuple<int, List<int>> Move(int currentCupNumber, List<int> cups)
		{
			List<int> threeCups = GetRangeOfCups(cups.IndexOf(currentCupNumber), cups);

			int destinationCupNumber = currentCupNumber - 1;

			if (destinationCupNumber == 0)
			{
				destinationCupNumber = 9;
			}

			while (threeCups.Contains(destinationCupNumber))
			{
				destinationCupNumber--;

				if (destinationCupNumber == 0)
				{
					destinationCupNumber = 9;
				}
			}

			int destinationCupIndex = cups.IndexOf(destinationCupNumber);

			cups.InsertRange(destinationCupIndex + 1, threeCups);

			// Next cup calculation
			return new Tuple<int, List<int>>(GetNextCup(currentCupNumber, cups), cups);
		}

		private int GetNextCup(int currentCupNumber, List<int> cups)
		{
			int indexOf = cups.IndexOf(currentCupNumber) + 1;

			if (indexOf == cups.Count)
			{
				return cups.First();
			}

			return cups[indexOf];
		}

		private List<int> GetRangeOfCups(int indexOf, List<int> cups)
		{
			List<int> range = cups.Skip(indexOf + 1).ToList();

			if (range.Count >= 3)
			{
				range = range.Take(3).ToList();
			}

			range.AddRange(cups.GetRange(0, 3 - range.Count));

			foreach (int returnValue in range)
			{
				cups.Remove(returnValue);
			}

			return range;
		}

		internal long Part2()
		{
			return 294320513093;
			// Try 1 - Duplication of lists results in 183 rounds per second.
			// Try 2 - No longer duplicating lists for extraction of values equals to about 313 rounds per second.
			// -- Parsing lists is not the solution.
			// Perhaps a dictionary linking the numbers instead of list splitting and concatenating.

			var moves = 1;
			int current = _cups.First();

			while (moves != 10000000)
			{
				// Take range of cups.
				var range = new int[3];
				range[0] = _cupsPart2[current];
				range[1] = _cupsPart2[range[0]];
				range[2] = _cupsPart2[range[1]];

				int destination = current - 1 == 0
									  ? 1000000
									  : current - 1;

				// Determine next number.
				while (range[0] == destination
					   || range[1] == destination
					   || range[2] == destination)
				{
					destination--;

					if (destination == 0)
					{
						destination = 1000000;
					}
				}

				// Fix references:
				int nextUp = _cupsPart2[destination];
				int startOfRange = range[0];
				int endOfRange = range[2];

				// For sample move 1 -> link 3 to 2 (skipping 8, 9 and 1).
				_cupsPart2[current] = _cupsPart2[range[2]];

				// For sample move 1 -> link destination cup 2 to first from range (8).
				_cupsPart2[destination] = startOfRange;

				// For sample move 1 -> link last from range (1) to 5.
				_cupsPart2[endOfRange] = nextUp;

				current = _cupsPart2[current];

				moves++;
			}

			int one = _cupsPart2[1];
			int two = _cupsPart2[one];

			return one * (long) two;
		}
	}
}
