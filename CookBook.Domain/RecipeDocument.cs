using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.Domain
{
    public class RecipeDocument
    {
        public Guid ID { get; set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Instructions { get; private set; }
        public string Ingredients { get; private set; }
        public int Servings { get; private set; }
        public bool IsFavourite { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public IEnumerable<string> PictureFileNames { get; private set; } = Enumerable.Empty<string>();
    }
}
