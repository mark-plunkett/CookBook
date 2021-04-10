using MediatR;
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
        public Task<IEnumerable<RecipeListDto>> Handle(RecipeListRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new[]
            {
                new RecipeListDto
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "test r"
                }
            }.AsEnumerable());
        }
    }
}
