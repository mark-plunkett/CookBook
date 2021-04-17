using MediatR;
using Raven.Client.Documents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CookBook.Domain.Commands
{
    public class GetPrimaryRecipeImageQueryHandler : IRequestHandler<GetPrimaryRecipeImageQuery, (Stream, string)>
    {
        private readonly IDocumentStore documentStore;

        public GetPrimaryRecipeImageQueryHandler(IDocumentStore documentStore)
        {
            this.documentStore = documentStore;
        }

        public async Task<(Stream, string)> Handle(GetPrimaryRecipeImageQuery request, CancellationToken cancellationToken)
        {
            using var session = this.documentStore.OpenAsyncSession();
            var album = await session.LoadAsync<RecipeAlbumDocument>(RecipeAlbumDocument.GetDocumentId(request.RecipeID));
            var attachments = session.Advanced.Attachments.GetNames(album);
            var attachment = await session.Advanced.Attachments.GetAsync(album, attachments.First().Name);
            return (attachment.Stream, attachment.Details.ContentType);
        }
    }
}
