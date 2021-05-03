using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CookBook.Domain.Recipes.Commands.CreateRecipeAlbumCommand;

namespace CookBook.Domain.Recipes.Commands
{
    public record CreateRecipeAlbumCommand(
        string DocumentPrefix, 
        IEnumerable<TempFile> Files) : IRequest<string>
    {
        public record TempFile(
            string FileName, 
            string ContentType, 
            Stream Stream);
    }
}
