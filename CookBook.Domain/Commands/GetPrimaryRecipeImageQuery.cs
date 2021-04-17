using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.Domain.Commands
{
    public record GetPrimaryRecipeImageQuery(Guid RecipeID) : IRequest<(Stream, string)>
    { }
}
