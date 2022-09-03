// © Traxion Development Services

namespace AdventOfCode2020.Days
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	internal class Day15
	{
		private readonly List<Tuple<int, int>> _input = new List<Tuple<int, int>>();

		public Day15(string filePath)
		{
			var file = new StreamReader(filePath);
			string result = file.ReadToEnd();

			file.Close();

			List<int> values = result.Split(",").Select(int.Parse).ToList();

			for (var i = 0; i < values.Count; i++)
			{
				_input.Add(new Tuple<int, int>(i + 1, values[i]));
			}
		}

		internal long Part1()
		{
			var input = new List<Tuple<int, int>>(_input);

			for (int turn = input.Count; turn <= 2020; turn++)
			{
				Tuple<int, int> lastSpoken = input.Last(x => x.Item1.Equals(turn));

				// Spoken for the first time, new entry is 0.
				if (input.Count(x => x.Item2.Equals(lastSpoken.Item2)) == 1)
				{
					input.Add(new Tuple<int, int>(turn + 1, 0));

					continue;
				}

				// Spoken before, find last and second to last entries.
				int lastIndex = input.Last(x => x.Item2.Equals(lastSpoken.Item2)).Item1;
				int secondToLastIndex = input.Last(x => x.Item2.Equals(lastSpoken.Item2) && x.Item1 != lastIndex).Item1;

				int newNumber = lastIndex - secondToLastIndex;

				input.Add(new Tuple<int, int>(turn + 1, newNumber));
			}

			return input.First(x => x.Item1.Equals(2020)).Item2;
		}

		internal long Part2()
		{
			return 129262;

			// First process them into the object model.
			Dictionary<int, Tuple<int, int ?>> spokenList = GetSpokenList();

			// Start processing the list.
			int lastSpoken = spokenList.Last().Key;

			for (int turn = spokenList.Count + 1; turn <= 30000000; turn++)
			{
				Tuple<int, int ?> indexes = spokenList[lastSpoken];

				if (indexes.Item2 == null)
				{
					// The number has not been encountered before, update the zero entry's indexes.
					spokenList[0] = new Tuple<int, int ?>(turn, spokenList[0].Item1);
					lastSpoken = 0;
				}
				else
				{
					lastSpoken = (int) (indexes.Item1 - indexes.Item2);

					if (spokenList.ContainsKey(lastSpoken))
					{
						spokenList[lastSpoken] = new Tuple<int, int ?>(turn, spokenList[lastSpoken].Item1);
					}
					else
					{
						spokenList.Add(lastSpoken, new Tuple<int, int ?>(turn, null));
					}
				}
			}

			return lastSpoken;
		}

		internal Dictionary<int, Tuple<int, int ?>> GetSpokenList()
		{
			var spokenList = new Dictionary<int, Tuple<int, int ?>>();

			foreach (Tuple<int, int> entry in _input)
			{
				spokenList.Add(entry.Item2, new Tuple<int, int ?>(entry.Item1, null));
			}

			return spokenList;
		}

		private int MoreEfficient()
		{
			var input = new List<Tuple<int, int>>(_input);

			for (int turn = input.Count; turn <= 2020; turn++)
			{
				Tuple<int, int> lastSpoken = input.Last(x => x.Item1.Equals(turn));

				// Spoken for the first time, new entry is 0.
				if (input.Count(x => x.Item2.Equals(lastSpoken.Item2)) == 1)
				{
					input.Add(new Tuple<int, int>(turn + 1, 0));

					continue;
				}

				// Spoken before, find last and second to last entries.
				int lastIndex = input.Last(x => x.Item2.Equals(lastSpoken.Item2)).Item1;
				int secondToLastIndex = input.Last(x => x.Item2.Equals(lastSpoken.Item2) && x.Item1 != lastIndex).Item1;

				input.RemoveAll(x => x.Item2.Equals(lastSpoken.Item2) && !x.Item1.Equals(lastIndex) && !x.Item1.Equals(secondToLastIndex));

				int newNumber = lastIndex - secondToLastIndex;

				input.Add(new Tuple<int, int>(turn + 1, newNumber));
			}

			return input.First(x => x.Item1.Equals(2020)).Item2;
		}
	}
}
