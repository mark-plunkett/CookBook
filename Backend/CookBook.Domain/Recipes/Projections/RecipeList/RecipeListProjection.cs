using MediatR;
using Raven.Client.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CookBook.Domain.Recipes.Projections.RecipeList
{
    public class RecipeListProjection : IRequestHandler<RecipeListRequest, IEnumerable<Recipe>>
    {
        private readonly IDocumentStore documentStore;

        public RecipeListProjection(
            IDocumentStore documentStore)
        {
            this.documentStore = documentStore;
        }

        public async Task<IEnumerable<Recipe>> Handle(RecipeListRequest request, CancellationToken cancellationToken)
        {
            using var session = documentStore.OpenAsyncSession();
            return await session.Query<Recipe>()
                .OrderByDescending(r => r.CreatedOn)
                .ToListAsync();
        }
    }
}
