using CookBook.Infrastructure;

namespace CookBook.Domain.Recipes.Events
{
    public record RecipeUntagged(string CanonicalizedTag) : IDomainEvent
    { }
}