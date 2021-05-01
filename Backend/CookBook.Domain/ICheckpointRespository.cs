using EventStore.ClientAPI;
using System.Threading.Tasks;

namespace CookBook.Domain
{
    public interface ICheckpointRespository
    {
        Task<long?> Get(string key);
        Task Save(string key, long position);
    }
}