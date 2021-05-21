using CookBook.Infrastructure;
using MediatR;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Attachments;
using Raven.Client.Documents.Session;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CookBook.Domain.Recipes.Commands
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
            var recipe = await session.LoadAsync<Recipe>(IdentityExtensions.GetDocumentID<Recipe>(request.RecipeID));
            if (!recipe.PictureFileNames.Any())
                return (Stream.Null, null);

            var albumDocumentID = RecipeAlbumDocument.GetDocumentId(request.RecipeID);
            var primaryImageName = recipe.PictureFileNames.First();
            var extIndex = primaryImageName.LastIndexOf(".");
            var scaledImageName = $"{primaryImageName.Substring(0, extIndex)}.{request.Width}x{request.Height}{primaryImageName.Substring(extIndex)}";
            var scaledImageExists = await session.Advanced.Attachments.ExistsAsync(albumDocumentID, scaledImageName);
            if (!scaledImageExists)
                await ScaleImage(
                    session,
                    request,
                    albumDocumentID,
                    primaryImageName,
                    scaledImageName);

            var attachment = await session.Advanced.Attachments.GetAsync(albumDocumentID, scaledImageName);
            return (attachment.Stream, attachment.Details.ContentType);
        }

        private async Task ScaleImage(
            IAsyncDocumentSession session,
            GetPrimaryRecipeImageQuery request,
            string albumDocumentID,
            string primaryImageName,
            string scaledImageName)
        {
            var attachment = await session.Advanced.Attachments.GetAsync(albumDocumentID, primaryImageName);
            using var image = Image.Load(attachment.Stream, out var type);
            var ratioX = (double)request.Width / image.Width;
            var ratioY = (double)request.Height / image.Height;
            var ratio = Math.Max(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var x = (int)Math.Max(0, newWidth - request.Width) / 2;
            var y = (int)Math.Max(0, newHeight - request.Height) / 2;
            var size = Math.Min(newWidth, newHeight);

            image.Mutate(img => img.
                Resize(newWidth, newHeight)
                .Crop(new Rectangle(x, y, size, size)));

            using var memoryStream = new MemoryStream();
            image.Save(memoryStream, type);
            memoryStream.Position = 0;
            session.Advanced.Attachments.Store(albumDocumentID, scaledImageName, memoryStream, attachment.Details.ContentType);
            await session.SaveChangesAsync();
        }
    }
}
