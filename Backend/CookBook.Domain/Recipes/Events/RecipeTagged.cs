using CookBook.Infrastructure;

namespace CookBook.Domain.Recipes.Events
{
    public record RecipeTagged(string CanonicalizedTag) : IDomainEvent
    { }
}