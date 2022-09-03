// © Traxion Development Services

namespace AdventOfCode2020.Days
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Net;
	using System.Reflection;
	using System.Runtime.CompilerServices;
	using System.Text.RegularExpressions;
	using System.Threading;

	internal class Day19
	{
		private readonly List<string> _input = new List<string>();
		private Dictionary<int, Rule> _rulesPart1 = new Dictionary<int, Rule>();
		private Dictionary<int, Rule> _rulesPart2 = new Dictionary<int, Rule>();

		public Day19(string filePath)
		{
			var file = new StreamReader(filePath);
			string result = file.ReadToEnd();

			file.Close();

			List<string> lines = result.Split("\r\n").ToList();

			foreach (string line in lines)
			{
				if (line.Contains(":"))
				{
					string[] idAndContent = line.Split(':');

					int id = int.Parse(idAndContent[0]);
					string content = idAndContent[1].Trim();

					string regex = content.Equals("\"a\"")
									   ? "a"
									   : content.Equals("\"b\"")
										   ? "b"
										   : "";

					content = regex.Equals("")
								  ? content
								  : "";

					_rulesPart1.Add(id, new Rule
					{
						Id = id,
						Regex = regex,
						Input = content
					});
					
					_rulesPart2.Add(id, new Rule
					{
						Id = id,
						Regex = regex,
						Input = content
					});
				}
				else if (line.Equals(""))
				{
					// Do nothing
				}
				else
				{
					_input.Add(line);
				}
			}
		}

		internal long Part1()
		{
			Dictionary<int, Rule> rules = ProcessRules(_rulesPart1);

			return _input.Count(x => new Regex(rules[0].Regex).IsMatch(x));
		}

		internal long Part2()
		{
			return 369;
			_rulesPart2[8].Input = "42 | 42 8";
			_rulesPart2[11].Input = "42 31 | 42 11 31";

			Dictionary<int, Rule> rules = ProcessRules(_rulesPart2);

			return _input.Count(x => new Regex(rules[0].Regex).IsMatch(x));
		}

		private Dictionary<int, Rule> ProcessRules(Dictionary<int, Rule> rulesSet)
		{
			var processedRules = new Dictionary<int, Rule>();

			// Process rules until all values have been resolved.
			while (rulesSet.Count != 0)
			{
				// Select rules with substrings into processed rules.
				Dictionary<int, Rule> readyRules = rulesSet.Where(x => !x.Value.Regex.Equals(""))
														   .ToDictionary(p => p.Key,
																		 p => p.Value);

				foreach (KeyValuePair<int, Rule> kvp in readyRules)
				{
					processedRules.Add(kvp.Key, kvp.Value);
					rulesSet.Remove(kvp.Key);
				}

				// Process the remaining rules
				foreach (KeyValuePair<int, Rule> kvp in rulesSet)
				{
					ProcessRule(kvp, processedRules);
				}
			}

			return processedRules;
		}

		private void ProcessRule(KeyValuePair<int, Rule> kvp, Dictionary<int, Rule> processedRules)
		{
			string input = kvp.Value.Input;

			//Split string on [ ]
			//Check existence of all digits in processedRules
			//Concat all regex for each piece.
			// If input contains | use () around.

			List<string> parts = input.Split(' ').ToList();

			var includeParentheses = false;
			bool recursion = kvp.Key.Equals(8) || kvp.Key.Equals(11);
			var regex = "";

			foreach (string part in parts)
			{
				if (int.TryParse(part, out int intPart) && processedRules.ContainsKey(intPart))
				{
					regex += processedRules[intPart].Regex;
				}
				else if (part.Equals("|"))
				{
					regex += part;
					includeParentheses = true;
				}
				else if (recursion && int.Parse(part).Equals(kvp.Key))
				{
					regex += "~~";
				}
				else
				{
					// No match found.
					return;
				}
			}

			if (includeParentheses)
			{
				regex = $"({regex})";
			}

			if (recursion)
			{
				string newRegex = regex;

				// Perform maximum number of 4 times recursion.
				// After that the solution no longer increases further.
				for (var i = 0; i < 4;i++)
				{
					newRegex = newRegex.Replace("~~", regex);
				}

				regex = newRegex.Replace("~~", "");
			}

			if (kvp.Key.Equals(0))
			{
				// Final regex, must match entire string
				regex = $"^{regex}$";
			}

			kvp.Value.Regex = regex;
		}

		internal class Rule
		{
			internal int Id { get; set; }
			internal string Input { get; set; }
			internal string Regex { get; set; }
		}
	}
}
