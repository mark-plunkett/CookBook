using CookBook.Infrastructure;

namespace CookBook.Domain.Rules
{
    internal class RecipeMustHaveATitleRule : IBusinessRule
    {
        private readonly string s;

        public bool IsViolated => string.IsNullOrWhiteSpace(this.s);

        public string Message => "Recipe must have a title.";

        public RecipeMustHaveATitleRule(string s)
        {
            this.s = s;
        }
    }
}