using System.Threading.Tasks;
using CookBook.Domain.Tags.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly IMediator mediator;

        public TagsController(
            IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTagCommand command)
        {
            await this.mediator.Send(command);
            return Ok();
        }
    }
}
