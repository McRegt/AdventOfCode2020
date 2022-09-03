// © Traxion Development Services

namespace AdventOfCode2020.Days
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	internal class Day12
	{
		private readonly List<Tuple<string, int>> _input = new List<Tuple<string, int>>();

		// Considering maximum of a 360 degree turn.
		private readonly string _windDirections = "NESWNESWNESW";

		public Day12(string filePath)
		{
			var file = new StreamReader(filePath);
			string result = file.ReadToEnd();

			file.Close();

			result.Split("\r\n").ToList().ForEach(x => _input.Add(new Tuple<string, int>(x.Substring(0, 1), int.Parse(x.Substring(1, x.Length - 1)))));
		}

		internal long Part1()
		{
			// Calculate the distance for East and West movement.
			int north = _input.Where(x => x.Item1.Equals("N")).Select(x => x.Item2).Sum();
			int south = _input.Where(x => x.Item1.Equals("S")).Select(x => x.Item2).Sum();

			// Calculate the distance for North and South movement.
			int east = _input.Where(x => x.Item1.Equals("E")).Select(x => x.Item2).Sum();
			int west = _input.Where(x => x.Item1.Equals("W")).Select(x => x.Item2).Sum();

			// Starting point is east.
			List<Tuple<string, int>> remainingActions = _input.Where(x => x.Item1.Equals("L") || x.Item1.Equals("R") || x.Item1.Equals("F")).ToList();

			var currentDirection = "E";

			foreach (Tuple<string, int> action in remainingActions)
			{
				if (action.Item1.Equals("F"))
				{
					switch (currentDirection)
					{
						case "N":
							north += action.Item2;

							break;
						case "E":
							east += action.Item2;

							break;
						case "S":
							south += action.Item2;

							break;
						case "W":
							west += action.Item2;

							break;
					}
				}

				if (action.Item1.Equals("R"))
				{
					int quarterTurns = action.Item2 / 90;

					int startIndex = _windDirections.IndexOf(currentDirection, 4, StringComparison.Ordinal);
					currentDirection = _windDirections.Substring(startIndex + quarterTurns, 1);
				}

				if (action.Item1.Equals("L"))
				{
					int quarterTurns = action.Item2 / 90;

					int startIndex = _windDirections.IndexOf(currentDirection, 4, StringComparison.Ordinal);
					currentDirection = _windDirections.Substring(startIndex - quarterTurns, 1);
				}
			}

			return Math.Abs(west - east) + Math.Abs(north - south);
		}

		internal long Part2()
		{
			var wayPointWestEast = 10;
			var wayPointNorthSouth = 1;

			var shipPositionWestEast = 0;
			var shipPosistionNorthSouth = 0;

			foreach (Tuple<string, int> action in _input)
			{
				switch (action.Item1)
				{
					case "N":
						wayPointNorthSouth += action.Item2;

						break;
					case "E":
						wayPointWestEast += action.Item2;

						break;
					case "S":
						wayPointNorthSouth -= action.Item2;

						break;
					case "W":
						wayPointWestEast -= action.Item2;

						break;
					case "R":
						int beforeNS = wayPointNorthSouth;
						int beforeWE = wayPointWestEast;

						if (action.Item2 == 90)
						{
							wayPointWestEast = beforeNS;
							wayPointNorthSouth = beforeWE * -1;
						}

						if (action.Item2 == 180)
						{
							wayPointNorthSouth = beforeNS * -1;
							wayPointWestEast = beforeWE * -1;
						}

						if (action.Item2 == 270)
						{
							wayPointWestEast = beforeNS * -1;
							wayPointNorthSouth = beforeWE;
						}

						break;
					case "L":
						int beforeNSL = wayPointNorthSouth;
						int beforeWEL = wayPointWestEast;

						if (action.Item2 == 90)
						{
							wayPointWestEast = beforeNSL * -1;
							wayPointNorthSouth = beforeWEL;
						}

						if (action.Item2 == 180)
						{
							wayPointNorthSouth = beforeNSL * -1;
							wayPointWestEast = beforeWEL * -1;
						}

						if (action.Item2 == 270)
						{
							wayPointWestEast = beforeNSL;
							wayPointNorthSouth = beforeWEL * -1;
						}

						break;
					case "F":
						shipPosistionNorthSouth += wayPointNorthSouth * action.Item2;
						shipPositionWestEast += wayPointWestEast * action.Item2;

						break;
				}
			}

			return Math.Abs(shipPosistionNorthSouth) + Math.Abs(shipPositionWestEast);
		}
	}
}
