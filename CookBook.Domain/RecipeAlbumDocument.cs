using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.Domain
{
    record RecipeAlbumDocument(string Id)
    {
        public static RecipeAlbumDocument Create(Guid guid)
        {
            return new RecipeAlbumDocument($"RecipeAlbum-{guid}");
        }

        public static RecipeAlbumDocument Create()
        {
            return Create(Guid.NewGuid());
        }
    }
}
