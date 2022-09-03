// © Traxion Development Services

namespace AdventOfCode2020.Days
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	internal class Day13
	{
		private readonly int _earliestTimestamp;
		private readonly List<int> _busIdsPart1 = new List<int>();
		private readonly List<long> _busIdsPart2 = new List<long>();

		public Day13(string filePath)
		{
			var file = new StreamReader(filePath);
			string result = file.ReadToEnd();

			file.Close();

			string[] lines = result.Split("\r\n");

			_earliestTimestamp = int.Parse(lines[0]);

			foreach (string busId in lines[1].Split(','))
			{
				if (busId != "x")
				{
					_busIdsPart1.Add(int.Parse(busId));
					_busIdsPart2.Add(long.Parse(busId));
				}
				else
				{
					_busIdsPart2.Add(0);
				}
			}
		}

		internal long Part1()
		{
			var busWaitDictionary = new Dictionary<int, int>();

			foreach (int busId in _busIdsPart1)
			{
				int remainder = _earliestTimestamp % busId;
				int waitTime = busId - remainder;

				busWaitDictionary.Add(busId, waitTime);
			}

			KeyValuePair<int, int> keyAndValue = busWaitDictionary.OrderBy(x => x.Value).First();

			return keyAndValue.Key * keyAndValue.Value;
		}

		internal long Part2()
		{
			var busStop = new ProcessedBusStop
			{
				StartIndex = _busIdsPart2[0],
				StepCount = _busIdsPart2[0]
			};

			for (var i = 1; i < _busIdsPart2.Count; i++)
			{
				busStop = CalculateBusStop(_busIdsPart2[i], i, busStop);
			}

			return busStop.StartIndex;
		}

		private ProcessedBusStop CalculateBusStop(long busId, long offset, ProcessedBusStop busStop)
		{
			// Empty bus line means continuing with the next bus
			if (busId == 0)
			{
				return busStop;
			}

			var resultFound = false;
			long newStartIndex = 0;

			while (!resultFound)
			{
				long result = (busStop.StartIndex + offset) % busId;

				if (result == 0)
				{
					resultFound = true;
					newStartIndex = busStop.StartIndex;
				}
				else
				{
					busStop.StartIndex += busStop.StepCount;
				}
			}

			return new ProcessedBusStop
			{
				StartIndex = newStartIndex,
				StepCount = busStop.StepCount * busId
			};
		}

		internal class ProcessedBusStop
		{
			internal long StartIndex { get; set; }
			internal long StepCount { get; set; }
		}
	}
}
