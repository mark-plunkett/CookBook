using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.Domain.Commands
{
    public record FavouriteRecipeCommand(
        Guid RecipeID,
        bool IsFavourite)
        : IRequest
    { }
}
