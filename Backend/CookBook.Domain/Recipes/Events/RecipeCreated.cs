using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookBook.Infrastructure;

namespace CookBook.Domain.Recipes.Events
{
    public record RecipeCreated(
        Guid RecipeID,
        string Title,
        string Description,
        string Instructions,
        string Ingredients,
        int Servings,
        DateTime CreatedOn)
        : IDomainEvent
    { }
}
