using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Raven.Client.Documents;

namespace CookBook.Domain.Tags.Queries
{
    public class ListTagsQueryHandler : IRequestHandler<ListTagsQueryRequest, IEnumerable<Tag>>
    {
        private readonly IDocumentStore documentStore;

        public ListTagsQueryHandler(IDocumentStore documentStore)
        {
            this.documentStore = documentStore;
        }

        public async Task<IEnumerable<Tag>> Handle(ListTagsQueryRequest request, CancellationToken cancellationToken)
        {
            using var session = documentStore.OpenAsyncSession();
            return await session.Query<Tag>()
                .OrderBy(t => t.Name)
                .ToListAsync();
        }
    }
}
