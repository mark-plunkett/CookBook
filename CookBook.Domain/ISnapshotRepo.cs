using System.Threading.Tasks;

namespace CookBook.Domain
{
    public interface ISnapshotRepo
    {
        Task Save<TAggregate>(TAggregate aggregate) where TAggregate : Aggregate;
    }
}