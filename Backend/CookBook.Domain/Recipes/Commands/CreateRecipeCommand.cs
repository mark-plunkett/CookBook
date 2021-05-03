using CookBook.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.Domain.Recipes.Commands
{
    public class CreateRecipeCommand : IRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Instructions { get; set; }
        public string Ingredients { get; set; }
        public int Servings { get; set; }
        public string RecipeAlbumDocumentID { get; set; }
    }
}
