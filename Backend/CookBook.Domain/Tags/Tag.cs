
using System;
using CookBook.Infrastructure;

namespace CookBook.Domain.Tags
{
    public class Tag : Aggregate
    {
        public string Name { get; private set; }
        public string Canonicalized { get; private set; }

        protected override void When(IDomainEvent @event)
        {
            switch (@event)
            {
                case TagCreated e: OnCreated(e); break;
            };
        }

        public void Create(string name)
        {
            var trimmed = name.Trim();
            BusinessRule.Enforce(new TagMustHaveNameRule(trimmed));

            base.Apply(new TagCreated(trimmed, trimmed.Canonicalize()));
        }

        public void OnCreated(TagCreated e)
        {
            this.Name = e.Name;
            this.Canonicalized = e.CanonicalizedName;
        }
    }
}