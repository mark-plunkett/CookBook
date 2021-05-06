
using System;
using CookBook.Domain.Rules;
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

        public void Create(Guid id, string name)
        {
            BusinessRule.Enforce(new IDMustBeNonDefaultRule(id));
            var trimmed = name.Trim();
            BusinessRule.Enforce(new TagMustHaveNameRule(trimmed));

            base.Apply(new TagCreated(id, trimmed, trimmed.Canonicalize()));
        }

        public void OnCreated(TagCreated e)
        {
            base.ID = e.TagID;
            this.Name = e.Name;
            this.Canonicalized = e.CanonicalizedName;
        }
    }
}