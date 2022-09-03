// © Traxion Development Services

namespace AdventOfCode2020.Days
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text.RegularExpressions;

	internal class Day07
	{
		private readonly Dictionary<string, Dictionary<string, int>> _input = new Dictionary<string, Dictionary<string, int>>();

		public Day07(string filePath)
		{
			var file = new StreamReader(filePath);
			string result = file.ReadToEnd();

			file.Close();

			string[] lines = result.Split("\r\n");

			foreach (string line in lines)
			{
				if (line.Contains("contain no other bags."))
				{
					continue;
				}

				string[] kvp = line.Split("contain");

				string key = kvp[0].Replace("bags", "").Trim();

				string[] values = kvp[1].Trim().Replace(".", "").Split(",");

				var colors = new Dictionary<string, int>();

				foreach (string value in values)
				{
					int numberOfBags = int.Parse(Regex.Match(value.Trim(), @"^\d+").ToString());
					string strippedValue = Regex.Replace(value.Trim(), @"^\d+", "");
					strippedValue = strippedValue.Replace("bags", "").Replace("bag", "").Trim();

					colors.Add(strippedValue, numberOfBags);
				}

				_input.Add(key, colors);
			}
		}

		internal int Part1()
		{
			return AllowedBagColors("shiny gold").Distinct().Count();
		}

		private List<string> AllowedBagColors(string color)
		{
			var allowedColors = new List<string>();
			List<string> additionalColors = _input.Where(x => x.Value.ContainsKey(color)).Select(x => x.Key).ToList();

			foreach (string additionalColor in additionalColors)
			{
				allowedColors.Add(additionalColor);
				allowedColors.AddRange(AllowedBagColors(additionalColor));
			}

			return allowedColors;
		}

		internal int Part2()
		{
			int result = BagsIncludedInBag("shiny gold");

			return result;
		}

		private int BagsIncludedInBag(string color)
		{
			var bagCount = 0;
			List<Dictionary<string, int>> includedBags = _input.Where(x => x.Key.Equals(color)).Select(x => x.Value).ToList();

			foreach (Dictionary<string, int> includedBag in includedBags)
			{
				foreach (KeyValuePair<string, int> kvp in includedBag)
				{
					int bagsIncluded = BagsIncludedInBag(kvp.Key);

					bagCount += kvp.Value;
					bagCount += kvp.Value * bagsIncluded;
				}
			}

			return bagCount;
		}
	}
}
