// © Traxion Development Services

namespace AdventOfCode2020.Days
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	internal class Day06
	{
		private readonly List<string> _input;

		public Day06(string filePath)
		{
			var file = new StreamReader(filePath);
			string result = file.ReadToEnd();

			file.Close();

			_input = result.Split("\r\n\r\n").ToList();
			_input = _input.Select(x => x.Replace("\r\n", " ")).ToList();
		}

		internal int Part1()
		{
			var totalYes = 0;

			foreach (string group in _input)
			{
				string combined = group.Replace(" ", "");
				totalYes += combined.Distinct().ToArray().Length;
			}

			return totalYes;
		}

		internal int Part2()
		{
			var totalYes = 0;

			foreach (string group in _input)
			{
				string[] individuals = group.Split(" ");
				var sameAnswers = new List<char>();

				for (var i = 0; i < individuals.Length; i++)
				{
					// The first entry in a group is the basis.
					if (i == 0)
					{
						sameAnswers = individuals[i].Distinct().ToList();

						continue;
					}

					// All other members can only remove items from the everyone list
					List<char> answers = individuals[i].Distinct().ToList();

					foreach (char sameAnswer in sameAnswers.ToList())
					{
						if (!answers.Contains(sameAnswer))
						{
							sameAnswers.Remove(sameAnswer);
						}
					}
				}

				totalYes += sameAnswers.Count;
			}

			return totalYes;
		}
	}
}
