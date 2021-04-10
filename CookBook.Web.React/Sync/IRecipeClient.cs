using CookBook.Domain;
using System.Threading.Tasks;

namespace CookBook.Web.React.Sync
{
    public interface IRecipeClient
    {
        Task RecieveRecipe(Recipe recipe);
    }
}