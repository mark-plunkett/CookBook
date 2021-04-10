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
        public Task SendRecipe(Recipe recipe)
        {
            return Clients.All.RecieveRecipe(recipe);
        }
    }
}
