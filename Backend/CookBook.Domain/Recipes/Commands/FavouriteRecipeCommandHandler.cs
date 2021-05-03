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
    public class FavouriteRecipeCommandHandler : AsyncRequestHandler<FavouriteRecipeCommand>
    {
        private readonly IAggregateRepository aggregateRepository;

        public FavouriteRecipeCommandHandler(
            IAggregateRepository aggregateRepository)
        {
            this.aggregateRepository = aggregateRepository;
        }

        protected override async Task Handle(FavouriteRecipeCommand request, CancellationToken cancellationToken)
        {
            var agg = await this.aggregateRepository.LoadAsync<Recipe>(request.RecipeID);
            if (request.IsFavourite)
                agg.Favourite();
            else
                agg.Unfavourite();

            await this.aggregateRepository.SaveAsync(agg);
        }
    }
}
