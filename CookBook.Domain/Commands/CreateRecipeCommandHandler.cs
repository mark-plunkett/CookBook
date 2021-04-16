using MediatR;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Attachments;
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
        private readonly IDocumentStore documentStore;

        public CreateRecipeCommandHandler(
            IAggregateRepository aggregateRepository,
            IDocumentStore documentStore)
        {
            this.aggregateRepository = aggregateRepository;
            this.documentStore = documentStore;
        }

        protected override async Task Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid();
            var pictures = await MovePictureFiles(id, request.RecipeAlbumDocumentID);

            var agg = await this.aggregateRepository.LoadAsync<Recipe>(id);
            agg.Create(
                id,
                request.Title,
                request.Description,
                request.Instructions,
                request.Ingredients,
                request.Servings);

            foreach (var picture in pictures)
                agg.AttachPicture(picture.Name);

            await this.aggregateRepository.SaveAsync(agg);
        }

        private async Task<IEnumerable<AttachmentName>> MovePictureFiles(Guid recipeID, string albumDocumentID)
        {
            if (string.IsNullOrEmpty(albumDocumentID))
                return Enumerable.Empty<AttachmentName>();

            using var session = this.documentStore.OpenAsyncSession();
            var newAlbum = RecipeAlbumDocument.Create(recipeID);
            if (await session.Advanced.ExistsAsync(newAlbum.Id))
                return Enumerable.Empty<AttachmentName>();

            await session.StoreAsync(newAlbum);
            var tempAlbum = await session.LoadAsync<RecipeAlbumDocument>(albumDocumentID);
            foreach (var a in session.Advanced.Attachments.GetNames(tempAlbum))
                session.Advanced.Attachments.Move(tempAlbum, a.Name, newAlbum, a.Name);

            session.Delete(albumDocumentID);
            await session.SaveChangesAsync();
            return session.Advanced.Attachments.GetNames(newAlbum);
        }
    }
}
