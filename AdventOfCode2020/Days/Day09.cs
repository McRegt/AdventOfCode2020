// © Traxion Development Services

namespace AdventOfCode2020.Days
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	internal class Day09
	{
		private readonly List<long> _input;
		private long _part1Answer;

		public Day09(string filePath)
		{
			var file = new StreamReader(filePath);
			string result = file.ReadToEnd();

			file.Close();

			_input = result.Split("\r\n").Select(long.Parse).ToList();
		}

		internal long Part1()
		{
			var numberOfNumbers = 25;
			var startIndex = 0;

			_part1Answer = LookupRangeOfSums(startIndex, numberOfNumbers);

			return _part1Answer;
		}

		public long LookupRangeOfSums(int currentIndex, int numberOfNumbers)
		{
			List<long> rangeOfNumbers = _input.GetRange(currentIndex, numberOfNumbers);
			long numberToFind = _input[currentIndex + numberOfNumbers];

			var sumFound = false;

			for (var i = 0; i < rangeOfNumbers.Count; i++)
			{
				long valueOne = _input[i + currentIndex];

				for (var j = 0; j < rangeOfNumbers.Count; j++)
				{
					// Skip the comparison if the index is the same
					if (i == j)
					{
						continue;
					}

					long valueTwo = _input[j + currentIndex];

					if (valueOne + valueTwo == numberToFind)
					{
						sumFound = true;

						break;
					}
				}

				if (sumFound)
				{
					break;
				}
			}

			if (!sumFound)
			{
				return numberToFind;
			}

			currentIndex = currentIndex + 1;

			return LookupRangeOfSums(currentIndex, numberOfNumbers);
		}

		internal long Part2()
		{
			_input.RemoveRange(_input.IndexOf(_part1Answer), _input.Count - _input.IndexOf(_part1Answer));

			int lastIndex = _input.Count - 1;

			List<long> result = SumValuesUntilBufferOverflow(lastIndex);

			return result.Min() + result.Max();
		}

		private List<long> SumValuesUntilBufferOverflow(int startIndex)
		{
			var summedItems = new List<long>();

			// Loop through the list backwards as the items are 'roughly' in order.
			for (int i = startIndex; i > 0; i--)
			{
				summedItems.Add(_input[i]);

				// Get the sum of all items in the list.
				long sum = summedItems.Sum();

				if (sum.Equals(_part1Answer))
				{
					// We found the resulting list of numbers summing to the expected answer.
					break;
				}

				if (sum > _part1Answer)
				{
					// Overflow detected, we must start with a lower start index.
					return SumValuesUntilBufferOverflow(startIndex - 1);
				}

				// Otherwise just continue with adding the next (really previous item).
			}

			return summedItems;
		}
	}
}
