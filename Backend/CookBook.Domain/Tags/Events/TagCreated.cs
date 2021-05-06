using System;
using CookBook.Infrastructure;

namespace CookBook.Domain.Tags
{
    public record TagCreated(
        Guid TagID,
        string Name,
        string CanonicalizedName) 
        : IDomainEvent
    { }
}