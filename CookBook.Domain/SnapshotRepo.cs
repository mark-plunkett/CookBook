using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.Domain
{
    public class SnapshotRepo : ISnapshotRepo
    {
        private readonly IAsyncDocumentSession session;

        public SnapshotRepo(
            IAsyncDocumentSession session)
        {
            this.session = session;
        }

        public async Task Save<TAggregate>(TAggregate aggregate) where TAggregate : Aggregate
        {
            var doc = await this.session.LoadAsync<TAggregate>(aggregate.DocumentID);
            if (doc != null && doc.Version == aggregate.Version)
                return;

            await this.session.StoreAsync(aggregate, aggregate.DocumentID);
            await this.session.SaveChangesAsync();
        }
    }
}
