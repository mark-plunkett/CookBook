using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.Model.Commands
{
    public class CreateRecipeCommand
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Instructions { get; set; }
        public string Ingredients { get; set; }
        public int Servings { get; set; }
    }
}
