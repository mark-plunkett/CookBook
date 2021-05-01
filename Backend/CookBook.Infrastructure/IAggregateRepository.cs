using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.Infrastructure
{
    public interface IAggregateRepository
    {
        Task<T> LoadAsync<T>(Guid aggregateId) where T : Aggregate, new();
        Task SaveAsync<T>(T aggregate) where T : Aggregate, new();
    }
}
