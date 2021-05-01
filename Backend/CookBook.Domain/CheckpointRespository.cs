using EventStore.ClientAPI;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.Domain
{
    public class CheckpointRespository : ICheckpointRespository
    {
        private readonly IAsyncDocumentSession session;

        public CheckpointRespository(
            IAsyncDocumentSession session)
        {
            this.session = session;
        }

        static string GetFullKey(string key) => $"{key}-checkpoint";

        public async Task<long?> Get(string key)
        {
            var doc = await session.LoadAsync<CheckpointDocument>(GetFullKey(key));
            return doc?.Position;
        }

        public async Task Save(string key, long position)
        {
            var doc = new CheckpointDocument
            {
                Position = position
            };

            await session.StoreAsync(doc, GetFullKey(key));
            await session.SaveChangesAsync();
        }
    }
}
