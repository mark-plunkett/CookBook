using System;
using CookBook.Infrastructure;

namespace CookBook.Domain.Recipes.Rules
{
    internal class RecipeMustHaveAlbumRule : IBusinessRule
    {
        private readonly string albumID;

        public bool IsViolated => string.IsNullOrEmpty(this.albumID);

        public string Message => "Recipe must have at least one picture.";

        public RecipeMustHaveAlbumRule(string albumID)
        {
            this.albumID = albumID;
        }
    }
}