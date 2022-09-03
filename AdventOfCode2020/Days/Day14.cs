// © Traxion Development Services

namespace AdventOfCode2020.Days
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text.RegularExpressions;

	internal class Day14
	{
		private readonly List<MaskSequence> _input = new List<MaskSequence>();

		public Day14(string filePath)
		{
			var file = new StreamReader(filePath);
			string result = file.ReadToEnd();

			file.Close();

			List<string> lines = result.Split("\r\n").ToList();

			MaskSequence maskSequence = null;

			for (var i = 0; i < lines.Count; i++)
			{
				string line = lines[i];
				string[] values = line.Split('=');

				if (line.StartsWith("mask"))
				{
					if (maskSequence != null)
					{
						_input.Add(maskSequence);
					}

					maskSequence = new MaskSequence
					{
						Mask = values[1].Trim(),
						Values = new List<Tuple<long, long>>()
					};
				}

				if (line.StartsWith("mem"))
				{
					int memoryPosition = int.Parse(new Regex(@"(?<=\[).+?(?=\])").Matches(values[0])[0].Value);
					long value = long.Parse(values[1].Trim());

					// ReSharper disable once PossibleNullReferenceException
					maskSequence.Values.Add(new Tuple<long, long>(memoryPosition, value));
				}
			}

			// Add the last entry that was only constructed.
			_input.Add(maskSequence);
		}

		internal long Part1()
		{
			var part1Dictionary = new Dictionary<long, long>();

			foreach (MaskSequence maskSequence in _input)
			{
				Dictionary<int, int> maskValue = GetMaskValue(maskSequence.Mask.Reverse().ToArray());

				foreach (Tuple<long, long> positionValue in maskSequence.Values)
				{
					var bitArray = new BitArray(BitConverter.GetBytes(positionValue.Item2));

					foreach (KeyValuePair<int, int> bitValue in maskValue)
					{
						// Skip negative values, those are for part 2;
						if (bitValue.Value == 0
							|| bitValue.Value == 1)
						{
							bitArray[bitValue.Key] = Convert.ToBoolean(bitValue.Value);
						}
					}

					var array = new byte[8];
					bitArray.CopyTo(array, 0);
					var value = BitConverter.ToInt64(array);

					if (part1Dictionary.ContainsKey(positionValue.Item1))
					{
						part1Dictionary[positionValue.Item1] = value;
					}
					else
					{
						part1Dictionary.Add(positionValue.Item1, value);
					}
				}
			}

			return part1Dictionary.Values.Sum();
		}

		private Dictionary<int, int> GetMaskValue(char[] reverseMask)
		{
			var returnValue = new Dictionary<int, int>();

			for (var i = 0; i < reverseMask.Length; i++)
			{
				char character = reverseMask[i];

				switch (character)
				{
					case '1':
						returnValue.Add(i, 1);

						break;
					case '0':
						returnValue.Add(i, 0);

						break;
					case 'X':
						returnValue.Add(i, -1);

						break;
				}
			}

			return returnValue;
		}

		internal long Part2()
		{
			var returnValue = new Dictionary<long, long>();

			foreach (MaskSequence maskSequence in _input)
			{
				Dictionary<int, int> maskValue = GetMaskValue(maskSequence.Mask.Reverse().ToArray());

				foreach (Tuple<long, long> kvp in maskSequence.Values)
				{
					List<long> indexes = GetMemoryAddresses(kvp.Item1, maskValue);

					foreach (long index in indexes)
					{
						returnValue[index] = kvp.Item2;
					}
				}
			}

			return returnValue.Values.Sum();
		}

		internal List<long> GetMemoryAddresses(long initialMemoryAddress, Dictionary<int, int> maskValue)
		{
			var returnValue = new List<long>();

			var bitArray = new BitArray(BitConverter.GetBytes(initialMemoryAddress));

			var encodedMemoryAddress = new List<int>();

			for (var i = 0; i < maskValue.Count; i++)
			{
				var valueToAdd = 0;

				if (maskValue[i] == -1
					|| maskValue[i] == 1)
				{
					valueToAdd = maskValue[i];
				}
				else
				{
					// Use the value from the original.
					valueToAdd = Convert.ToInt32(bitArray[i]);
				}

				encodedMemoryAddress.Add(valueToAdd);
			}

			List<BitArray> allAddresses = CalculatePossibilities(encodedMemoryAddress);

			foreach (BitArray value in allAddresses)
			{
				var array = new byte[8];
				value.CopyTo(array, 0);
				var address = BitConverter.ToInt64(array, 0);

				returnValue.Add(address);
			}

			return returnValue;
		}

		internal List<BitArray> CalculatePossibilities(List<int> memoryAddress)
		{
			var listOfLists = new List<BitArray>();

			for (var i = 0; i < memoryAddress.Count; i++)
			{
				// When starting create the initial lists.
				if (i == 0)
				{
					if (memoryAddress[i] == 0
						|| memoryAddress[i] == -1)
					{
						var bitArray = new BitArray(36)
						{
							[i] = false
						};

						listOfLists.Add(bitArray);
					}

					if (memoryAddress[i] == 1
						|| memoryAddress[i] == -1)
					{
						var bitArray = new BitArray(36)
						{
							[i] = true
						};

						listOfLists.Add(bitArray);
					}
				}

				// Otherwise add entries to the lists.
				else
				{
					// Just add values in case of a regular number
					if (memoryAddress[i] == 0)
					{
						listOfLists.ForEach(x => x[i] = false);
					}

					if (memoryAddress[i] == 1)
					{
						listOfLists.ForEach(x => x[i] = true);
					}

					// If the value is floating, duplicate the lists.
					// Add 1 to the original, add 0 to the copy.
					// Combine the two afterwards.
					if (memoryAddress[i] == -1)
					{
						var newListofLists = new List<BitArray>();

						foreach (BitArray array in listOfLists)
						{
							var newList = array.Clone() as BitArray;
							newList[i] = false;
							newListofLists.Add(newList);

							var newList2 = array.Clone() as BitArray;
							newList2[i] = true;
							newListofLists.Add(newList2);
						}

						listOfLists = newListofLists;
					}
				}
			}

			return listOfLists;
		}

		internal class MaskSequence
		{
			internal string Mask { get; set; }
			internal List<Tuple<long, long>> Values { get; set; }
		}
	}
}
