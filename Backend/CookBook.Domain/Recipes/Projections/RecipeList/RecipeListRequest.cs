using MediatR;
using System.Collections.Generic;

namespace CookBook.Domain.Recipes.Projections.RecipeList
{
    public class RecipeListRequest : IRequest<IEnumerable<Recipe>>
    { }
}