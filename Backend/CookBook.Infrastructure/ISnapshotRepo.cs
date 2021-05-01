using System.Threading.Tasks;

namespace CookBook.Infrastructure
{
    public interface ISnapshotRepo
    {
        Task SaveAggregate<TAggregate>(TAggregate aggregate) where TAggregate : Aggregate;
    }
}