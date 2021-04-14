using System.Threading.Tasks;

namespace CookBook.Domain
{
    public interface ISnapshotRepo
    {
        Task SaveAggregate<TAggregate>(TAggregate aggregate) where TAggregate : Aggregate;
    }
}