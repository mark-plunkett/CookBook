using EventStore.ClientAPI;
using System.Threading.Tasks;

namespace CookBook.Domain
{
    public interface ICheckpointRespository
    {
        Task<Position?> Get(string key);
        Task Save(string key, Position position);
    }
}