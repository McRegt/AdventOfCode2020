// © Traxion Development Services

namespace AdventOfCode2020.Days
{
	using System;
	using System.Collections.Generic;
	using System.IO;

	internal class Day03
	{
		private readonly List<string> _input;

		public Day03(string filePath)
		{
			var file = new StreamReader(filePath);

			_input = new List<string>();

			string line;

			while ((line = file.ReadLine()) != null)
			{
				_input.Add(line);
			}

			file.Close();
		}

		internal int Part1()
		{
			var treeCounter = 0;

			for (var i = 0; _input.Count != i; i++)
			{
				if (i == 0)
				{
					// Skip first line as it is the starting point.
					continue;
				}

				// Calculate step within line
				int stepWithinLine = i * 3 % _input[i].Length;

				char result = _input[i][stepWithinLine];

				if (result.Equals('#'))
				{
					treeCounter++;
				}
			}

			return treeCounter;
		}

		internal long Part2()
		{
			return CheckSlope(1, 1) * CheckSlope(3, 1) * CheckSlope(5, 1) * CheckSlope(7, 1) * CheckSlope(1, 2);
		}

		private long CheckSlope(int rightStepSize, int downStepSize)
		{
			var treeCounter = 0;
			var stepCounter = 0;

			for (var i = 0; i < _input.Count; i += downStepSize)
			{
				// Calculate step within line without duplicating the line.
				int indexInLine = stepCounter * rightStepSize % _input[i].Length;

				char result = _input[i][indexInLine];

				if (result.Equals('#'))
				{
					treeCounter++;
				}

				stepCounter++;
			}

			return Convert.ToInt64(treeCounter);
		}
	}
}
