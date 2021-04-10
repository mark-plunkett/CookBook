using System;

namespace CookBook.Domain.Projections.RecipeList
{
    public class RecipeListDto
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
    }
}