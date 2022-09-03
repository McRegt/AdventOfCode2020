// © Traxion Development Services

namespace AdventOfCode2020.Days
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	internal class Day21
	{
		private readonly List<Product> _products = new List<Product>();
		private readonly List<Allergen> _allergens = new List<Allergen>();

		public Day21(string filePath)
		{
			var file = new StreamReader(filePath);
			string result = file.ReadToEnd();

			file.Close();

			result.Split("\r\n")
				  .ToList()
				  .ForEach(x =>
				  {
					  string[] splitted = x.Split(" (contains");

					  _products.Add(new Product
					  {
						  Ingredients = splitted[0].Split(' ').Select(x => x.Trim()).ToList(),
						  Allergens = splitted[1].Split(',').Select(x => x.Trim(' ', ')')).ToList()
					  });
				  });

			_products.SelectMany(x => x.Allergens)
					 .Distinct()
					 .ToList()
					 .ForEach(y => _allergens.Add(new Allergen
					 {
						 Name = y,
						 PossibleIngredients = _products.First(x => x.Allergens.Contains(y)).Ingredients.ToList()
					 }));
		}

		internal long Part1()
		{
			List<string> ingredients = _products.SelectMany(x => x.Ingredients).ToList();

			// Continue looping until we have the single ingredient.
			while (_allergens.Any(x => x.PossibleIngredients.Count != 0))
			{
				foreach (Allergen allergen in _allergens.Where(x => x.Ingredient == null))
				{
					foreach (Product product in _products.Where(x => x.Allergens.Contains(allergen.Name)))
					{
						foreach (string ingredient in allergen.PossibleIngredients.ToList())
						{
							if (!product.Ingredients.Contains(ingredient))
							{
								allergen.PossibleIngredients.Remove(ingredient);
							}
						}

						if (allergen.PossibleIngredients.Count == 1)
						{
							allergen.Ingredient = allergen.PossibleIngredients[0];

							// The ingredient can be removed from the ohter allergens possibilities as well.
							_allergens.ForEach(x => x.PossibleIngredients.Remove(allergen.Ingredient));

							break;
						}
					}
				}
			}

			List<string> allergens = _allergens.Select(x => x.Ingredient).ToList();

			allergens.ForEach(x => ingredients.RemoveAll(y => y == x));

			return ingredients.Count();
		}

		internal string Part2()
		{
			return string.Join(',', _allergens.OrderBy(x => x.Name).Select(x => x.Ingredient).ToList());
		}

		internal class Product
		{
			internal List<string> Ingredients { get; set; }
			internal List<string> Allergens { get; set; }
		}

		internal class Allergen
		{
			internal string Name { get; set; }
			internal List<string> PossibleIngredients { get; set; }
			internal string Ingredient { get; set; }
		}
	}
}
