using CookBook.Domain.Tags;
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
        private readonly ITagRepo tagRepo;

        public UpdateRecipeCommandHandler(
            IAggregateRepository aggregateRepository,
            ITagRepo tagRepo)
        {
            this.aggregateRepository = aggregateRepository;
            this.tagRepo = tagRepo;
        }

        protected override async Task Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
        {
            var canonicalized = request.Tags.Select(StringExtensions.Canonicalize);
            var tags = await this.tagRepo.CreateTags(canonicalized);
            var recipe = await this.aggregateRepository.LoadAsync<Recipe>(request.ID);
            recipe.Update(
                request.Title,
                request.Description,
                request.Instructions,
                request.Ingredients,
                request.Servings);

            foreach (var tag in tags)
                recipe.Tag(tag.Canonicalized);
            
            foreach (var removedTag in recipe.Tags.Except(canonicalized).ToList())
                recipe.Untag(removedTag);
            
            await this.aggregateRepository.SaveAsync(recipe);
        }
    }
}
