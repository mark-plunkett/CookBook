using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.Domain.Recipes.Commands
{
    public record GetPrimaryRecipeImageQuery(
        Guid RecipeID,
        int Width,
        int Height)
        : IRequest<(Stream, string)>
    { }
}
