using CookBook.Infrastructure;

namespace CookBook.Domain.Recipes.Rules
{
    public class AggregateMustBeNewRule : IBusinessRule
    {
        private readonly long version;

        public bool IsViolated => this.version >= 0;

        public string Message => "Aggregate already exists.";

        public AggregateMustBeNewRule(long version)
        {
            this.version = version;
        }
    }
}