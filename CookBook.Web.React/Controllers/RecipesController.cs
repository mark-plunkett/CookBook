using CookBook.Domain;
using CookBook.Domain.Commands;
using CookBook.Domain.Projections;
using CookBook.Domain.Projections.RecipeList;
using EventStore.ClientAPI;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.Web.React.Controllers
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
        public async Task<IEnumerable<RecipeListDto>> List()
        {
            return await this.mediator.Send(new RecipeListRequest());
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateRecipeCommand command)
        {
            await this.mediator.Send(command);
            return Ok();
        }

        [HttpPatch("{id}/favourite")]
        public async Task<IActionResult> Favourite(Guid id)
        {
            await this.mediator.Send(new FavouriteRecipeCommand(id, true));
            return Ok();
        }

        [HttpPatch("{id}/unfavourite")]
        public async Task<IActionResult> Unfavourite(Guid id)
        {
            await this.mediator.Send(new FavouriteRecipeCommand(id, false));
            return Ok();
        }
    }
}
