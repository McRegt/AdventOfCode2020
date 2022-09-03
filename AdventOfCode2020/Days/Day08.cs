// © Traxion Development Services

namespace AdventOfCode2020.Days
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	internal class Day08
	{
		private readonly List<string> _input;

		public Day08(string filePath)
		{
			var file = new StreamReader(filePath);
			string result = file.ReadToEnd();

			file.Close();

			_input = result.Split("\r\n").ToList();
		}

		internal int Part1()
		{
			var actionIndex = 0;

			var executedIndexes = new List<int>();

			var accumulator = 0;

			while (!executedIndexes.Contains(actionIndex))
			{
				executedIndexes.Add(actionIndex);

				Tuple<int, int> result = ReturnNextActionAndAccumulatorChange(_input, actionIndex);
				actionIndex = result.Item1;
				accumulator += result.Item2;
			}

			return accumulator;
		}

		private Tuple<int, int> ReturnNextActionAndAccumulatorChange(List<string> input, int currentIndex)
		{
			string[] actionValue = input[currentIndex].Split(" ");

			switch (actionValue[0])
			{
				case "acc":
					return new Tuple<int, int>(currentIndex + 1, int.Parse(actionValue[1]));
				case "jmp":
					int jumpActions = int.Parse(actionValue[1]);

					return new Tuple<int, int>(currentIndex + jumpActions, 0);

				case "nop":
					return new Tuple<int, int>(currentIndex + 1, 0);
			}

			throw new ArgumentException("Unclear what to do right now?");
		}

		internal int Part2()
		{
			var accumulator = 0;
			var actionIndex = 0;
			var changedIndex = 0;

			// While the end of the input list has not been reached.
			while (actionIndex < _input.Count)
			{
				List<string> updatedInput = UpdateInputAtIndex(_input, changedIndex);

				accumulator = 0;
				actionIndex = 0;

				// Loop through the list, every iteration updating the next action;
				var executedIndexes = new List<int>();

				while (!executedIndexes.Contains(actionIndex)
					   && actionIndex < _input.Count)
				{
					executedIndexes.Add(actionIndex);

					Tuple<int, int> result = ReturnNextActionAndAccumulatorChange(updatedInput, actionIndex);
					actionIndex = result.Item1;
					accumulator += result.Item2;
				}

				changedIndex++;
			}

			return accumulator;
		}

		private List<string> UpdateInputAtIndex(List<string> input, int changeAtIndex)
		{
			List<string> returnValue = input.ToList();

			string value = returnValue[changeAtIndex];

			if (value.Contains("jmp"))
			{
				returnValue[changeAtIndex] = returnValue[changeAtIndex].Replace("jmp", "nop");
			}

			if (value.Contains("nop"))
			{
				returnValue[changeAtIndex] = returnValue[changeAtIndex].Replace("nop", "jmp");
			}

			return returnValue;
		}
	}
}
