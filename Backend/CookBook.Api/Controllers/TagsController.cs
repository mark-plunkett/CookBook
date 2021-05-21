using System.Collections.Generic;
using System.Threading.Tasks;
using CookBook.Domain.Tags;
using CookBook.Domain.Tags.Commands;
using CookBook.Domain.Tags.Queries;
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

        [HttpGet]
        public async Task<IEnumerable<Tag>> Get()
        {
            return await this.mediator.Send(new ListTagsQueryRequest());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTagCommand command)
        {
            await this.mediator.Send(command);
            return Ok();
        }
    }
}
