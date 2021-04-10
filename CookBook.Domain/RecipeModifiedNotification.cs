using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.Domain
{
    public class RecipeModifiedNotification : INotification
    {
        public Recipe Recipe { get; private set; }

        public RecipeModifiedNotification(Recipe recipe)
        {
            this.Recipe = recipe;
        }
    }
}
