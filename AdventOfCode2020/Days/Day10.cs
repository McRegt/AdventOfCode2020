// © Traxion Development Services

namespace AdventOfCode2020.Days
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	internal class Day10
	{
		private readonly List<long> _input;

		public Day10(string filePath)
		{
			var file = new StreamReader(filePath);
			string result = file.ReadToEnd();

			file.Close();

			_input = result.Split("\r\n").Select(long.Parse).ToList();

			// Insert device adapter.
			_input.Add(_input.Max() + 3);

			// Insert wall adapter.
			_input.Add(0);

			_input = _input.OrderBy(x => x).ToList();
		}

		internal long Part1()
		{
			long oneJoltDifferenceCount = 0;
			long threeJoltDifferenceCount = 0;

			for (var i = 0; i < _input.Count - 1; i++)
			{
				long currentJoltRating = _input[i];
				long nextJoltRating = _input[i + 1];
				long difference = nextJoltRating - currentJoltRating;

				switch (difference)
				{
					case 1:
						oneJoltDifferenceCount++;

						break;
					case 2:
						break;
					case 3:
						threeJoltDifferenceCount++;

						break;
					default:
						throw new ArgumentException($"The difference ({difference}) is unexpected.");
				}
			}

			return oneJoltDifferenceCount * threeJoltDifferenceCount;
		}

		internal long Part2()
		{
			List<long> sortedInput = _input.OrderBy(x => x).ToList();

			// Infinite recursion, so be it.
			long returnValue = CalculatePossiblePathsForward(0);

			return returnValue;
		}

		internal Dictionary<long, long> cachedRemainders = new Dictionary<long, long>();

		private long CalculatePossiblePathsForward(long joltValue)
		{
			// Shortcut the recursion by applying caching.
			// Using 0, 1, 2, 3, 6 as sequence as example.
			// When the result from 3 onwards has been found it will also be the same for all other recursions.
			if (cachedRemainders.ContainsKey(joltValue))
			{
				// Return the cached value instead.
				// For joltValue = 3, this means 1. As the step from 3 to the last value (6) will always be a single step.
				return cachedRemainders[joltValue];
			}

			// Catch the end of the loop for each path.
			if (joltValue == _input.Max())
			{
				return 1;
			}

			long returnValue = 0;

			if (_input.Contains(joltValue + 1))
			{
				// Continue with recursion if the one step jolt exists.
				// Add the result to all other combinations.
				returnValue += CalculatePossiblePathsForward(joltValue + 1);
			}

			if (_input.Contains(joltValue + 2))
			{
				// Continue with recursion if the two step jolt exists.
				// Add the result to all other combinations.
				returnValue += CalculatePossiblePathsForward(joltValue + 2);
			}

			if (_input.Contains(joltValue + 3))
			{
				// Continue with recursion if the one step jolt exists.
				// Add the result to all other combinations.
				returnValue += CalculatePossiblePathsForward(joltValue + 3);
			}

			// We reached the end of recursion for a given jolt value.
			// Store it in the cache for use in other 'paths'.
			if (!cachedRemainders.ContainsKey(joltValue))
			{
				cachedRemainders.Add(joltValue, returnValue);
			}

			return returnValue;
		}
	}
}
