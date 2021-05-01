using MediatR;
using System.Collections.Generic;

namespace CookBook.Domain.Projections.RecipeList
{
    public class RecipeListRequest : IRequest<IEnumerable<Recipe>>
    { }
}