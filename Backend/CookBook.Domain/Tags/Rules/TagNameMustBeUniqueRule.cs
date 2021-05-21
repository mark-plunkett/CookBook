
using System;
using System.Collections.Generic;
using CookBook.Infrastructure;

namespace CookBook.Domain.Tags.Rules
{
    public class TagNameMustBeUniqueRule : IBusinessRule
    {
        private readonly string canonicalizedName;
        private Func<string, bool> tagFinder;

        public bool IsViolated => this.tagFinder.Invoke(this.canonicalizedName);
        public string Message => "Tag must be unique.";
        public string PropertyName => nameof(Tag.Name);

        public TagNameMustBeUniqueRule(string canonicalizedName, Func<string, bool> tagFinder)
        {
            this.canonicalizedName = canonicalizedName;
            this.tagFinder = tagFinder;
        }
    }
}