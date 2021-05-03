using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookBook.Infrastructure;

namespace CookBook.Domain.Recipes.Events
{
    public record RecipeUpdated(
        Guid RecipeID,
        string Title,
        string Description,
        string Instructions,
        string Ingredients,
        int Servings)
        : IDomainEvent
    { }
}
