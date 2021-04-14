using CookBook.Domain;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookBook.Web.React.Sync
{
    public class RecipeHub : Hub<IRecipeClient>
    {
        public Task SendRecipeCreated(Recipe recipe)
        {
            return Clients.All.RecipeCreated(recipe);
        }

        public Task SendRecipeUpdated(Recipe recipe)
        {
            return Clients.All.RecipeUpdated(recipe);
        }
    }
}
