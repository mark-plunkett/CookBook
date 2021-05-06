using System;
using System.Threading;
using System.Threading.Tasks;
using CookBook.Infrastructure;
using MediatR;

namespace CookBook.Domain.Tags.Commands
{
    public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand>
    {
        private readonly IAggregateRepository aggregateRepository;

        public CreateTagCommandHandler(IAggregateRepository aggregateRepository)
        {
            this.aggregateRepository = aggregateRepository;
        }

        public async Task<Unit> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid();
            var tag = await this.aggregateRepository.LoadAsync<Tag>(id);
            tag.Create(id, request.Name);
            await this.aggregateRepository.SaveAsync(tag);
            return Unit.Value;
        }
    }
}