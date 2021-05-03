using MediatR;
using Raven.Client.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CookBook.Domain.Recipes.Commands
{
    public class CreateRecipeAlbumCommandHandler : IRequestHandler<CreateRecipeAlbumCommand, string>
    {
        private readonly IDocumentStore documentStore;

        public CreateRecipeAlbumCommandHandler(
            IDocumentStore documentStore)
        {
            this.documentStore = documentStore;
        }

        public async Task<string> Handle(CreateRecipeAlbumCommand request, CancellationToken cancellationToken)
        {
            var document = RecipeAlbumDocument.Create();
            using var session = this.documentStore.OpenAsyncSession();
            await session.StoreAsync(document);
            foreach (var file in request.Files)
            {
                session.Advanced.Attachments.Store(document, file.FileName, file.Stream, file.ContentType);
            }

            await session.SaveChangesAsync();
            return document.Id;
        }
    }
}
