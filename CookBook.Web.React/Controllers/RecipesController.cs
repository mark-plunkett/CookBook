using CookBook.Model;
using CookBook.Model.Commands;
using EventStore.ClientAPI;
using Microsoft.AspNetCore.Mvc;
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
        private readonly AggregateRepository aggregateRepository;
        private readonly IEventStoreConnection eventStoreConnection;

        public RecipesController(
            AggregateRepository aggregateRepository,
            IEventStoreConnection eventStoreConnection)
        {
            this.aggregateRepository = aggregateRepository;
            this.eventStoreConnection = eventStoreConnection;
        }

        [HttpGet]
        public async Task<IEnumerable<RecipeDto>> List()
        {

            return Enumerable.Empty<RecipeDto>();
        }

        static string AccountIDToString(Guid id) => $"account-{id}";
        async Task ReadAll(string streamName)
        {
            Console.WriteLine(nameof(ReadAll));
            var events = await this.eventStoreConnection.ReadStreamEventsForwardAsync(streamName, StreamPosition.Start, 4096, resolveLinkTos: false);
            foreach (var e in events.Events)
                Console.WriteLine($"{e.Event.EventType} {Encoding.UTF8.GetString(e.Event.Data.ToArray())}");

            Console.WriteLine();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateRecipeCommand command)
        {
            var id = Guid.NewGuid();
            var agg = await this.aggregateRepository.LoadAsync<Recipe>(id);
            agg.Create(command);
            await this.aggregateRepository.SaveAsync(agg);
            return Ok();
        }
    }
}
