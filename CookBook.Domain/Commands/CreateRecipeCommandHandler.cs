using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CookBook.Domain.Commands
{
    public class CreateRecipeCommandHandler : AsyncRequestHandler<CreateRecipeCommand>
    {
        private readonly IAggregateRepository aggregateRepository;

        public CreateRecipeCommandHandler(
            IAggregateRepository aggregateRepository)
        {
            this.aggregateRepository = aggregateRepository;
        }

        protected override async Task Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid();
            var agg = await this.aggregateRepository.LoadAsync<Recipe>(id);
            agg.Create(request);
            await this.aggregateRepository.SaveAsync(agg);
        }
    }
}
