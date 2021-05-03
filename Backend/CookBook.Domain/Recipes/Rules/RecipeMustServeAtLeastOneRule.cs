using CookBook.Infrastructure;

namespace CookBook.Domain.Recipes.Rules
{
    internal class RecipeMustServeAtLeastOneRule : IBusinessRule
    {
        private readonly int servings;

        public bool IsViolated => this.servings < 1;

        public string Message => "Recipe must serve at least 1.";

        public RecipeMustServeAtLeastOneRule(int servings)
        {
            this.servings = servings;
        }
    }
}