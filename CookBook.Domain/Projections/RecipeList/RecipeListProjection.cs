using MediatR;
using Raven.Client.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CookBook.Domain.Projections.RecipeList
{
    public class RecipeListProjection : IRequestHandler<RecipeListRequest, IEnumerable<RecipeListDto>>
    {
        private readonly IDocumentStore documentStore;

        public RecipeListProjection(
            IDocumentStore documentStore)
        {
            this.documentStore = documentStore;
        }

        public async Task<IEnumerable<RecipeListDto>> Handle(RecipeListRequest request, CancellationToken cancellationToken)
        {
            using var session = documentStore.OpenAsyncSession();
            return await session.Query<Recipe>()
                .Select(r => new RecipeListDto
                {
                    ID = r.ID,
                    Title = r.Title
                })
                .ToListAsync();
        }
    }
}
