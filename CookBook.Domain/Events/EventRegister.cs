using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.Domain.Events
{
    public static class EventRegister
    {
        public static readonly IEnumerable<Type> Events = new HashSet<Type>(new[]
        {
            typeof(RecipeCreated),
            typeof(RecipeFavourited),
            typeof(RecipePictureAttached)
        });
    }
}
