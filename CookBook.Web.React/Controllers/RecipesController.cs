using CookBook.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookBook.Web.React.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipesController : ControllerBase
    {
        private readonly AggregateRepository aggregateRepository;

        public RecipesController(
            AggregateRepository aggregateRepository)
        {
            this.aggregateRepository = aggregateRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<RecipeDto>> List()
        {
            //return Enumerable.Empty<RecipeDto>();
            return new[]
            {
                new RecipeDto
                {
                    ID = Guid.NewGuid(),
                    Title = "Test Recipe"
                }
            };
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(
            [FromForm] string title,
            [FromForm] string description,
            [FromForm] string instructions,
            [FromForm] string ingredients,
            [FromForm] int servings)
        {
            var id = Guid.NewGuid();
            var agg = await this.aggregateRepository.LoadAsync<Recipe>(id);
            agg.Create(title, description, instructions, ingredients, servings);
            await this.aggregateRepository.SaveAsync(agg);
            return Ok();
        }
    }
}
