using CookBook.Infrastructure;

namespace CookBook.Domain.Tags
{
    public class TagMustHaveNameRule : IBusinessRule
    {
        private readonly string name;

        public bool IsViolated => string.IsNullOrWhiteSpace(this.name);

        public string Message => "Must have a name.";

        public TagMustHaveNameRule(string name)
        {
            this.name = name;
        }
    }
}