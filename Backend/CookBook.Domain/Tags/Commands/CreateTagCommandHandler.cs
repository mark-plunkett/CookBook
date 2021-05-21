using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CookBook.Infrastructure;
using MediatR;
using Raven.Client.Documents;

namespace CookBook.Domain.Tags.Commands
{
    public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand>
    {
        private readonly IDocumentStore documentStore;

        public CreateTagCommandHandler(
            IDocumentStore documentStore)
        {
            this.documentStore = documentStore;
        }

        public async Task<Unit> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            using var ravenSession = this.documentStore.OpenAsyncSession();
            var tag = Tag.Create(request.Name);
            await ravenSession.StoreAsync(tag); 
            await ravenSession.SaveChangesAsync();
            return Unit.Value;
        }
    }
}