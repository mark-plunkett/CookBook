using CookBook.Infrastructure;

namespace CookBook.Domain.Tags.Rules
{
    public class TagMustHaveNameRule : IBusinessRule
    {
        private readonly string name;

        public bool IsViolated => string.IsNullOrWhiteSpace(this.name);

        public string Message => "Must have a name.";

        public string PropertyName => nameof(Tag.Name);

        public TagMustHaveNameRule(string name)
        {
            this.name = name;
        }
    }
}