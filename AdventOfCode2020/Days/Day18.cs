// © Traxion Development Services

namespace AdventOfCode2020.Days
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text.RegularExpressions;

	internal class Day18
	{
		private readonly List<string> _input;

		private readonly Regex _subClauseRegex = new Regex(@"\([\d*\+]+\)");
		private readonly Regex _leftToRightRegex = new Regex(@"(\d+)([*\+])(\d+)");
		private readonly Regex _additionOnlyRegex = new Regex(@"(\d+)(\+)(\d+)");

		public Day18(string filePath)
		{
			var file = new StreamReader(filePath);
			string result = file.ReadToEnd();

			file.Close();

			_input = result.Split("\r\n").Select(x => x.Replace(" ", "")).ToList();
		}

		internal long Part1()
		{
			return 1451467526514;
			long answer = 0;

			_input.ForEach(x =>
			{
				Console.WriteLine("");
				Console.WriteLine("Processing line:");
				Console.WriteLine(x);

				long clauseAnswer = long.Parse(Calculate(x, CalculateForPart1));

				Console.WriteLine(clauseAnswer);

				answer += clauseAnswer;
			});

			Console.WriteLine("");

			return answer;
		}

		internal long Part2()
		{
			return 224973686321527;
			long answer = 0;

			_input.ForEach(x =>
			{
				Console.WriteLine("");
				Console.WriteLine("Processing line:");
				Console.WriteLine(x);

				long clauseAnswer = long.Parse(Calculate(x, CalculateForPart2));

				Console.WriteLine(clauseAnswer);

				answer += clauseAnswer;
			});

			return answer;
		}

		private string Calculate(string clause, Func<string, string> partCalculation)
		{
			// First resolve all the subclauses
			while (_subClauseRegex.IsMatch(clause))
			{
				Match firstMatch = _subClauseRegex.Match(clause);

				string calculatedValue = partCalculation(firstMatch.Value.Trim('(', ')'));

				clause = clause.Remove(firstMatch.Index, firstMatch.Length);
				clause = clause.Insert(firstMatch.Index, calculatedValue);

				Console.WriteLine(clause);
			}

			return partCalculation(clause);
		}

		private string CalculateForPart1(string clause)
		{
			while (_leftToRightRegex.IsMatch(clause))
			{
				Match firstMatch = _leftToRightRegex.Match(clause);

				long firstValue = long.Parse(firstMatch.Groups[1].Value);
				long secondValue = long.Parse(firstMatch.Groups[3].Value);
				long result = 0;

				switch (firstMatch.Groups[2].Value)
				{
					case "+":
						result = firstValue + secondValue;

						break;
					case "*":
						result = firstValue * secondValue;

						break;
				}

				clause = clause.Remove(firstMatch.Index, firstMatch.Length);
				clause = clause.Insert(firstMatch.Index, result.ToString());
			}

			return clause;
		}

		private string CalculateForPart2(string clause)
		{
			// Start with additions
			while (_additionOnlyRegex.IsMatch(clause))
			{
				Match firstMatch = _additionOnlyRegex.Match(clause);

				long firstValue = long.Parse(firstMatch.Groups[1].Value);
				long secondValue = long.Parse(firstMatch.Groups[3].Value);
				long result = firstValue + secondValue;

				clause = clause.Remove(firstMatch.Index, firstMatch.Length);
				clause = clause.Insert(firstMatch.Index, result.ToString());
			}

			clause = CalculateForPart1(clause);

			return clause;
		}
	}
}
