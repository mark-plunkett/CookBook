using System;

namespace CookBook.Domain.Recipes.Projections.RecipeList
{
    public class RecipeListDto
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public bool IsFavourite { get; set; }
    }
}