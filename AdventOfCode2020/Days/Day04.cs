// © Traxion Development Services

namespace AdventOfCode2020.Days
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.IO;
	using System.Linq;

	public class IdentityCardPart1
	{
		[Required]
		public string BirthYear { get; set; }

		[Required]
		public string IssueYear { get; set; }

		[Required]
		public string ExpirationYear { get; set; }

		[Required]
		public string Height { get; set; }

		[Required]
		public string HairColor { get; set; }

		[Required]
		public string EyeColor { get; set; }

		[Required]
		public string PasswordId { get; set; }

		public string CountryId { get; set; }
	}

	public class IdentityCardPart2
	{
		[Required]
		[Range(1920, 2002)]
		public string BirthYear { get; set; }

		[Required]
		[Range(2010, 2020)]
		public string IssueYear { get; set; }

		[Required]
		[Range(2020, 2030)]
		public string ExpirationYear { get; set; }

		[Required]
		[ValidHeight]
		public string Height { get; set; }

		[Required]
		[RegularExpression("^#[a-f0-9]{6}$")]
		public string HairColor { get; set; }

		[Required]
		[RegularExpression("^(amb|blu|brn|gry|grn|hzl|oth)$")]
		public string EyeColor { get; set; }

		[Required]
		[RegularExpression("^[0-9]{9}$")]
		public string PasswordId { get; set; }

		public string CountryId { get; set; }
	}

	public class ValidHeight : ValidationAttribute
	{
		public override bool IsValid(object objectValue)
		{
			var value = objectValue.ToString();

			if (!string.IsNullOrEmpty(value)
				&& value.EndsWith("cm"))
			{
				int heightInCm = int.Parse(value.Replace("cm", ""));

				if (heightInCm >= 150
					&& heightInCm <= 193)
				{
					return true;
				}
			}

			if (!string.IsNullOrEmpty(value)
				&& value.EndsWith("in"))
			{
				int heightInCm = int.Parse(value.Replace("in", ""));

				if (heightInCm >= 59
					&& heightInCm <= 76)
				{
					return true;
				}
			}

			return false;
		}
	}

	internal class Day04
	{
		private readonly List<string> _input;

		public Day04(string filePath)
		{
			var file = new StreamReader(filePath);
			string result = file.ReadToEnd();

			file.Close();

			_input = result.Split("\r\n\r\n").ToList();
			_input = _input.Select(x => x.Replace("\r\n", " ")).ToList();
		}

		internal int Part1()
		{
			var validIdentities = new List<IdentityCardPart1>();

			foreach (string rawCard in _input)
			{
				var cardValues = new Dictionary<string, string>();
				rawCard.Split(" ").ToList().ForEach(x => cardValues.Add(x.Split(":")[0], x.Split(":")[1]));

				var identity = new IdentityCardPart1
				{
					BirthYear = cardValues.ContainsKey("byr")
									? cardValues["byr"]
									: null,
					IssueYear = cardValues.ContainsKey("iyr")
									? cardValues["iyr"]
									: null,
					ExpirationYear = cardValues.ContainsKey("eyr")
										 ? cardValues["eyr"]
										 : null,
					Height = cardValues.ContainsKey("hgt")
								 ? cardValues["hgt"]
								 : null,
					HairColor = cardValues.ContainsKey("hcl")
									? cardValues["hcl"]
									: null,
					EyeColor = cardValues.ContainsKey("ecl")
								   ? cardValues["ecl"]
								   : null,
					PasswordId = cardValues.ContainsKey("pid")
									 ? cardValues["pid"]
									 : null,
					CountryId = cardValues.ContainsKey("cid")
									? cardValues["cid"]
									: null
				};

				var context = new ValidationContext(identity, null, null);
				var errorResults = new List<ValidationResult>();

				if (Validator.TryValidateObject(identity,
												context,
												errorResults,
												true))
				{
					validIdentities.Add(identity);
				}
			}

			return validIdentities.Count;
		}

		internal int Part2()
		{
			var validIdentities = new List<IdentityCardPart2>();

			foreach (string rawCard in _input)
			{
				var cardValues = new Dictionary<string, string>();
				rawCard.Split(" ").ToList().ForEach(x => cardValues.Add(x.Split(":")[0], x.Split(":")[1]));

				var identity = new IdentityCardPart2
				{
					BirthYear = cardValues.ContainsKey("byr")
									? cardValues["byr"]
									: null,
					IssueYear = cardValues.ContainsKey("iyr")
									? cardValues["iyr"]
									: null,
					ExpirationYear = cardValues.ContainsKey("eyr")
										 ? cardValues["eyr"]
										 : null,
					Height = cardValues.ContainsKey("hgt")
								 ? cardValues["hgt"]
								 : null,
					HairColor = cardValues.ContainsKey("hcl")
									? cardValues["hcl"]
									: null,
					EyeColor = cardValues.ContainsKey("ecl")
								   ? cardValues["ecl"]
								   : null,
					PasswordId = cardValues.ContainsKey("pid")
									 ? cardValues["pid"]
									 : null,
					CountryId = cardValues.ContainsKey("cid")
									? cardValues["cid"]
									: null
				};

				var context = new ValidationContext(identity, null, null);
				var errorResults = new List<ValidationResult>();

				if (Validator.TryValidateObject(identity,
												context,
												errorResults,
												true))
				{
					validIdentities.Add(identity);
				}
			}

			return validIdentities.Count;
		}
	}
}
