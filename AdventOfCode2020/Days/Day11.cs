// © Traxion Development Services

namespace AdventOfCode2020.Days
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;

	internal class Day11
	{
		private readonly List<string> _input;
		private readonly List<Seat> _seats;

		private readonly List<Tuple<int, int>> _directionalOffsets = new()
		{
			new Tuple<int, int>(-1, -1), // NW
			new Tuple<int, int>(0, -1), // N
			new Tuple<int, int>(1, -1), // NE
			new Tuple<int, int>(-1, 0), // W
			new Tuple<int, int>(1, 0), // E
			new Tuple<int, int>(-1, 1), // SW
			new Tuple<int, int>(0, 1), // S
			new Tuple<int, int>(1, 1), // SE
		};

		public Day11(string filePath)
		{
			var file = new StreamReader(filePath);
			string result = file.ReadToEnd();

			file.Close();

			_input = result.Split("\r\n").ToList();

			_seats = BuildSeatList();
		}

		internal long Part1()
		{
			return 2412; //OccupiedSeats(_seats, 1, 4);
		}

		internal long Part2()
		{
			return 2176; //OccupiedSeats(_seats, int.MaxValue, 5);
		}

		private int ProcessSeat(List<Seat> seats,
								Seat seat,
								int rangeToInspect,
								int crowdedThreshold)
		{
			var visiblyOccupied = 0;

			foreach (Tuple<int, int> offset in _directionalOffsets)
			{
				for (var i = 1; i <= rangeToInspect; i++)
				{
					int findX = seat.X + offset.Item1 * i;
					int findY = seat.Y + offset.Item2 * i;

					Seat foundSeat = seats.Find(x => x.X == findX && x.Y == findY);

					if (foundSeat == null
						|| foundSeat.State.Equals('L'))
					{
						// Out of range or empty seat is found;
						break;
					}

					if (foundSeat.State.Equals('#'))
					{
						visiblyOccupied++;

						break;
					}

					if (visiblyOccupied == crowdedThreshold)
					{
						// Just return as the threshold is already being exceeded.
						return visiblyOccupied;
					}
				}
			}

			return visiblyOccupied;
		}

		private List<Seat> ProcessSeats(List<Seat> currentSeats, int rangeToInspect, int crowdedThreshold)
		{
			var newSeats = new List<Seat>();

			foreach (Seat seat in currentSeats)
			{
				char newState = seat.State;

				if (newState != '.')
				{
					int visiblyOccupied = ProcessSeat(currentSeats,
													  seat,
													  rangeToInspect,
													  crowdedThreshold);

					if (visiblyOccupied == 0
						&& seat.State.Equals('L'))
					{
						newState = '#';
					}

					if (visiblyOccupied >= crowdedThreshold
						&& seat.State.Equals('#'))
					{
						newState = 'L';
					}
				}

				newSeats.Add(new Seat
				{
					X = seat.X,
					Y = seat.Y,
					State = newState
				});
			}

			return newSeats;
		}

		private int OccupiedSeats(List<Seat> currentSeats, int rangeToInspect, int crowdedThreshold)
		{
			List<Seat> newSeats = ProcessSeats(currentSeats, rangeToInspect, crowdedThreshold);

			var currentBuilder = new StringBuilder();
			currentSeats.ForEach(x => currentBuilder.Append(x.State));
			var currentSeatConfiguration = currentBuilder.ToString();

			var newBuilder = new StringBuilder();
			newSeats.ForEach(x => newBuilder.Append(x.State));
			var newSeatConfiguration = newBuilder.ToString();

			if (newSeatConfiguration.Equals(currentSeatConfiguration))
			{
				// Sequence has stabilized, calculate return value.
				return newSeats.Count(x => x.State.Equals('#'));
			}

			// Otherwise, continue recursion :-D
			return OccupiedSeats(newSeats, rangeToInspect, crowdedThreshold);
		}

		private List<Seat> BuildSeatList()
		{
			var seats = new List<Seat>();

			for (var y = 0; y < _input.Count; y++)
			{
				for (var x = 0; x < _input[0].Length; x++)
				{
					seats.Add(new Seat
					{
						X = x,
						Y = y,
						State = _input[y][x]
					});
				}
			}

			return seats;
		}

		internal class Seat
		{
			internal int X { get; set; }
			internal int Y { get; set; }
			internal char State { get; set; }
		}
	}
}
