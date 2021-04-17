using CookBook.Domain;
using CookBook.Domain.Commands;
using CookBook.Domain.Projections;
using CookBook.Domain.Projections.RecipeList;
using EventStore.ClientAPI;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipesController : ControllerBase
    {
        private readonly IMediator mediator;

        public RecipesController(
            IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<Recipe>> List()
        {
            return await this.mediator.Send(new RecipeListRequest());
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateRecipeCommand command)
        {
            await this.mediator.Send(command);
            return Ok();
        }

        [HttpPatch("{id}/{action}")]
        public async Task<IActionResult> Favourite(Guid id)
        {
            await this.mediator.Send(new FavouriteRecipeCommand(id, true));
            return Ok();
        }

        [HttpPatch("{id}/{action}")]
        public async Task<IActionResult> Unfavourite(Guid id)
        {
            await this.mediator.Send(new FavouriteRecipeCommand(id, false));
            return Ok();
        }

        [HttpPost("{action}")]
        public async Task<IActionResult> UploadFiles(IEnumerable<IFormFile> files)
        {
            var command = new CreateRecipeAlbumCommand(
                nameof(Recipe),
                files.Select(f => new CreateRecipeAlbumCommand.TempFile(
                    f.FileName,
                    f.ContentType,
                    f.OpenReadStream())));
            var tempID = await this.mediator.Send(command);
            return Ok(tempID);
        }

        [HttpGet("{id}/{action}")]
        public async Task<IActionResult> PrimaryImage(Guid id, int width, int height)
        {
            var (stream, contentType) = await this.mediator.Send(new GetPrimaryRecipeImageQuery(id, width, height));
            return File(stream, contentType);
        }
    }
}
