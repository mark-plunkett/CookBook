using System;
using CookBook.Infrastructure;

namespace CookBook.Domain.Rules
{
    public class IDMustBeNonDefaultRule : IBusinessRule
    {
        private readonly Guid id;

        public bool IsViolated => this.id == Guid.Empty;

        public string Message => "ID must be provided.";

        public IDMustBeNonDefaultRule(Guid id)
        {
            this.id = id;
        }
    }
}
