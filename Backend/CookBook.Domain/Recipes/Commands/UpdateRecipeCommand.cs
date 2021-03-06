using MediatR;
using System;
using System.Collections.Generic;

namespace CookBook.Domain.Recipes.Commands
{
    public class UpdateRecipeCommand : IRequest
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Instructions { get; set; }
        public string Ingredients { get; set; }
        public int Servings { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}