using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookBook.Infrastructure;
using Raven.Client.Documents;

namespace CookBook.Domain.Tags
{
    public class TagRepo : ITagRepo
    {
        private readonly IDocumentStore documentStore;

        public TagRepo(IDocumentStore documentStore)
        {
            this.documentStore = documentStore;
        }

        public async Task<IEnumerable<Tag>> CreateTags(IEnumerable<string> tagNames)
        {
            using var session = this.documentStore.OpenAsyncSession();
            var tags = tagNames
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Select(Tag.Create)
                .ToList();
            foreach (var tag in tags)
            {
                if (await session.LoadAsync<Tag>(tag.Id) == null)
                    await session.StoreAsync(tag);
            }

            await session.SaveChangesAsync();
            return tags;
        }
    }
}