using System;
using System.Collections.Generic;
using System.Linq;
using CookBook.Domain.Recipes;
using CookBook.Domain.Rules;
using CookBook.Domain.Tags.Rules;
using CookBook.Infrastructure;

namespace CookBook.Domain.Tags
{
    public class Tag
    {
        private readonly HashSet<string> _recipeIDs = new HashSet<string>();

        public string Id => GetDocumentID(this.Canonicalized);
        public string Name { get; private set; }
        public string Canonicalized { get; private set; }
        public IEnumerable<string> RecipeIDs => _recipeIDs.AsEnumerable();

        public static string GetDocumentID(string canonicalized) => $"Tag-{canonicalized}";

        public static Tag Create(string name)
        {
            var trimmed = name.Trim();
            var canonicalized = trimmed.Canonicalize();
            BusinessRule.Enforce(new TagMustHaveNameRule(trimmed));

            return new Tag
            {
                Name = trimmed,
                Canonicalized = canonicalized
            };
        }

        public void AddRecipe(Recipe recipe)
        {
            if (!_recipeIDs.Contains(recipe.DocumentID))
                _recipeIDs.Add(recipe.DocumentID);
        }
    }
}