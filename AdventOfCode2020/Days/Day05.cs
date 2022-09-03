// © Traxion Development Services

namespace AdventOfCode2020.Days
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	internal class Day05
	{
		private readonly List<string> _input;
		private readonly List<int> _seatIds = new List<int>();

		public Day05(string filePath)
		{
			var file = new StreamReader(filePath);
			string result = file.ReadToEnd();

			file.Close();

			_input = result.Split("\r\n").ToList();
		}

		internal int Part1()
		{
			foreach (string seatValue in _input)
			{
				string rowRawValue = seatValue.Substring(0, 7);
				string columRawValue = seatValue.Substring(7, 3);

				var rowNumber = Convert.ToInt32(rowRawValue.Replace("B", "1").Replace("F", "0"), 2);
				var columnNumber = Convert.ToInt32(columRawValue.Replace("R", "1").Replace("L", "0"), 2);

				int seatId = rowNumber * 8 + columnNumber;

				_seatIds.Add(seatId);
			}

			return _seatIds.Max();
		}

		internal int Part2()
		{
			for (var mySeatId = 1; _seatIds.Count > mySeatId; mySeatId++)
			{
				if (!_seatIds.Contains(mySeatId)
					&& _seatIds.Contains(mySeatId - 1)
					&& _seatIds.Contains(mySeatId + 1))
				{
					return mySeatId;
				}
			}

			throw new ArgumentException("Seat number was not found.");
		}
	}
}
