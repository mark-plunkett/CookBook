using CookBook.Infrastructure;

namespace CookBook.Domain.Tags
{
    public record TagCreated(string Name, string CanonicalizedName) : IDomainEvent
    { }
}