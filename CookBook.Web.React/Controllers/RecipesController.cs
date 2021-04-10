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
        private readonly IDocumentStore documentStore;

        public RecipesController(
            IMediator mediator,
            IDocumentStore documentStore)
        {
            this.mediator = mediator;
            this.documentStore = documentStore;
        }

        [HttpGet]
        public async Task<IEnumerable<RecipeListDto>> List()
        {
            return await this.mediator.Send(new RecipeListRequest());
        }

        [HttpGet("raven")]
        public async Task<RecipeListDto> Raven()
        {
            var id = Guid.NewGuid();
            using (var session = this.documentStore.OpenSession())
            {
                var listDto = new RecipeListDto
                {
                    ID = id,
                    Title = "Recipe " + DateTime.UtcNow.ToString()
                };

                session.Store(listDto);

                session.SaveChanges();
            }

            using (var session = this.documentStore.OpenSession())
            {
                return session.Load<RecipeListDto>(id.ToString());
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateRecipeCommand command)
        {
            await this.mediator.Send(command);
            return Ok();
        }
    }
}
