using CookBook.Domain;
using System.Threading.Tasks;

namespace CookBook.Web.React.Sync
{
    public interface IRecipeClient
    {
        Task RecipeCreated(Recipe recipe);
        Task RecipeUpdated(Recipe recipe);
    }
}