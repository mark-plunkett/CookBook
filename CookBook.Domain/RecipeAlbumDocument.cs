using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.Domain
{
    record RecipeAlbumDocument(string Id)
    {
        public static string GetDocumentId(Guid recipeID) => $"RecipeAlbum-{recipeID}";

        public static RecipeAlbumDocument Create(Guid guid)
        {
            return new RecipeAlbumDocument(GetDocumentId(guid));
        }

        public static RecipeAlbumDocument Create()
        {
            return Create(Guid.NewGuid());
        }
    }
}
