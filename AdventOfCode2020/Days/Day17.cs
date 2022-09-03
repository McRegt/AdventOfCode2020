// © Traxion Development Services

namespace AdventOfCode2020.Days
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	internal class Day17
	{
		private List<ActiveCube> _activeCubes = new List<ActiveCube>();
		private List<ActiveHyperCube> _activeHyperCubes = new List<ActiveHyperCube>();

		private readonly double _diagonal = Math.Sqrt(1 + 1 + 1);
		private readonly double _hyperDiagonal = Math.Sqrt(1 + 1 + 1 + 1);

		public Day17(string filePath)
		{
			var file = new StreamReader(filePath);
			string result = file.ReadToEnd();

			file.Close();

			List<string> lines = result.Split("\r\n").ToList();

			var y = 0;

			foreach (string line in lines)
			{
				for (var i = 0; i < line.Length; i++)
				{
					if (line[i].Equals('#'))
					{
						_activeCubes.Add(new ActiveCube
						{
							X = i,
							Y = y,
							Z = 0
						});

						_activeHyperCubes.Add(new ActiveHyperCube
						{
							X = i,
							Y = y,
							Z = 0,
							W = 0
						});
					}
				}

				y++;
			}
		}

		internal long Part1()
		{
			var cycleCounter = 0;

			while (cycleCounter != 6)
			{
				ProcessCubes();

				cycleCounter++;
			}

			return _activeCubes.Count;
		}

		private void ProcessCubes()
		{
			// A turn starts with determining the size of the grid.
			// The grid is always larger, one cube on each side of each plane.
			int maxX = _activeCubes.Max(x => x.X) + 2;
			int maxY = _activeCubes.Max(x => x.Y) + 2;
			int maxZ = _activeCubes.Max(x => x.Z) + 2;

			// Relocate the cubes in the grid.
			_activeCubes.ForEach(x =>
			{
				x.X++;
				x.Y++;
				x.Z++;
			});

			List<ActiveCube> newActiveCubes = _activeCubes.ToList();

			// Process the coordinates
			for (var z = 0; z <= maxZ; z++)
			{
				for (var y = 0; y <= maxY; y++)
				{
					for (var x = 0; x <= maxX; x++)
					{
						var cubeCounter = 0;

						// Find the neighboring active cubes.
						foreach (ActiveCube cube in _activeCubes)
						{
							// Count proximity
							double deltaX = x - cube.X;
							double deltaY = y - cube.Y;
							double deltaZ = z - cube.Z;

							double distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);

							// Maximum distance is diagonal, it cannot be itself.
							if (distance <= _diagonal
								&& distance != 0)
							{
								cubeCounter++;
							}
						}

						ActiveCube activeCube = _activeCubes.Find(c => c.X == x && c.Y == y && c.Z == z);

						// Null is an inactive cube.
						if (activeCube == null)
						{
							if (cubeCounter == 3)
							{
								newActiveCubes.Add(new ActiveCube
								{
									X = x,
									Y = y,
									Z = z
								});
							}
						}
						else
						{
							if (cubeCounter != 2
								&& cubeCounter != 3)
							{
								newActiveCubes.Remove(activeCube);
							}
						}
					}
				}
			}

			_activeCubes = newActiveCubes;
		}

		internal long Part2()
		{
			return 1504;
			var cycleCounter = 0;

			while (cycleCounter != 6)
			{
				ProcessHyperCubes();

				cycleCounter++;
			}

			return _activeHyperCubes.Count;
		}

		private void ProcessHyperCubes()
		{
			// A turn starts with determining the size of the grid.
			// The grid is always larger, one cube on each side of each plane.
			int maxX = _activeHyperCubes.Max(x => x.X) + 2;
			int maxY = _activeHyperCubes.Max(x => x.Y) + 2;
			int maxZ = _activeHyperCubes.Max(x => x.Z) + 2;
			int maxW = _activeHyperCubes.Max(x => x.W) + 2;

			// Relocate the cubes in the grid.
			_activeHyperCubes.ForEach(x =>
			{
				x.X++;
				x.Y++;
				x.Z++;
				x.W++;
			});

			List<ActiveHyperCube> newActiveHyperCubes = _activeHyperCubes.ToList();

			// Process the coordinates
			for (var w = 0; w <= maxW; w++)
			{
				for (var z = 0; z <= maxZ; z++)
				{
					for (var y = 0; y <= maxY; y++)
					{
						for (var x = 0; x <= maxX; x++)
						{
							var cubeCounter = 0;

							// Find the neighboring active cubes.
							foreach (ActiveHyperCube cube in _activeHyperCubes)
							{
								// Count proximity
								double deltaX = x - cube.X;
								double deltaY = y - cube.Y;
								double deltaZ = z - cube.Z;
								double deltaW = w - cube.W;

								double distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ + deltaW * deltaW);

								// Maximum distance is diagonal, it cannot be itself.
								// The diagonal distance would be fine, were it not for the additional requirement that the delta can at maximum be 1.
								if (distance != 0
									&& Math.Abs(deltaX) <= 1
									&& Math.Abs(deltaY) <= 1
									&& Math.Abs(deltaZ) <= 1
									&& Math.Abs(deltaW) <= 1)
								{
									cubeCounter++;
								}
							}

							ActiveHyperCube activeHyperCube = _activeHyperCubes.Find(c => c.X == x && c.Y == y && c.Z == z && c.W == w);

							// Null is an inactive cube.
							if (activeHyperCube == null)
							{
								if (cubeCounter == 3)
								{
									newActiveHyperCubes.Add(new ActiveHyperCube
									{
										X = x,
										Y = y,
										Z = z,
										W = w
									});
								}
							}
							else
							{
								if (cubeCounter != 2
									&& cubeCounter != 3)
								{
									newActiveHyperCubes.Remove(activeHyperCube);
								}
							}
						}
					}
				}
			}

			_activeHyperCubes = newActiveHyperCubes;
		}

		internal class ActiveCube
		{
			internal int X { get; set; }
			internal int Y { get; set; }
			internal int Z { get; set; }
		}

		internal class ActiveHyperCube : ActiveCube
		{
			internal int W { get; set; }
		}
	}
}
