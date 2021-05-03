using CookBook.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CookBook.Domain.Recipes.Commands
{
    public class UpdateRecipeCommandHandler : AsyncRequestHandler<UpdateRecipeCommand>
    {
        private readonly IAggregateRepository aggregateRepository;

        public UpdateRecipeCommandHandler(
            IAggregateRepository aggregateRepository)
        {
            this.aggregateRepository = aggregateRepository;
        }

        protected override async Task Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
        {
            var agg = await this.aggregateRepository.LoadAsync<Recipe>(request.ID);
            agg.Update(
                request.Title,
                request.Description,
                request.Instructions,
                request.Ingredients,
                request.Servings);

            await this.aggregateRepository.SaveAsync(agg);
        }
    }
}
