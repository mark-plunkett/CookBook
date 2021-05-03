using CookBook.Domain.Recipes;
using System.Threading.Tasks;

namespace CookBook.Api.Sync
{
    public interface IRecipeClient
    {
        Task RecipeCreated(Recipe recipe);
        Task RecipeUpdated(Recipe recipe);
    }
}