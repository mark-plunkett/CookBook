using CookBook.Domain.Recipes.Commands;
using CookBook.Domain.Recipes.Events;
using CookBook.Domain.Recipes.Rules;
using CookBook.Domain.Rules;
using CookBook.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.Domain.Recipes
{
    public class Recipe : Aggregate
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Instructions { get; private set; }
        public string Ingredients { get; private set; }
        public int Servings { get; private set; }
        public bool IsFavourite { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public IEnumerable<string> PictureFileNames { get; private set; } = new List<string>();

        protected override void When(IDomainEvent @event)
        {
            switch (@event)
            {
                case RecipeCreated e: OnCreated(e); break;
                case RecipeUpdated e: OnUpdated(e); break;
                case RecipeFavourited e: OnFavourited(e); break;
                case RecipePictureAttached e: OnPictureAttached(e); break;
            }
        }

        public void Create(
            Guid id,
            string title,
            string description,
            string instructions,
            string ingredients,
            int servings)
        {
            BusinessRule.Enforce(new IDMustBeNonDefaultRule(id));
            BusinessRule.Enforce(new AggregateMustBeNewRule(base.Version));
            BusinessRule.Enforce(new RecipeMustHaveATitleRule(title), nameof(title));
            BusinessRule.Enforce(new RecipeMustServeAtLeastOneRule(servings), nameof(servings));

            base.Apply(new RecipeCreated(
                id,
                title,
                description,
                instructions,
                ingredients,
                servings,
                DateTime.UtcNow));
        }

        public void Update(
            string title,
            string description,
            string instructions,
            string ingredients,
            int servings)
        {
            BusinessRule.Enforce(new RecipeMustHaveATitleRule(title), nameof(title));
            BusinessRule.Enforce(new RecipeMustServeAtLeastOneRule(servings), nameof(servings));

            base.Apply(new RecipeUpdated(
                base.ID,
                title,
                description,
                instructions,
                ingredients,
                servings));
        }

        public void Favourite()
        {
            if (this.IsFavourite)
                return;

            base.Apply(new RecipeFavourited(this.ID, IsFavourite: true));
        }

        public void Unfavourite()
        {
            if (!this.IsFavourite)
                return;

            base.Apply(new RecipeFavourited(this.ID, IsFavourite: false));
        }

        public void AttachPicture(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            if (this.PictureFileNames.Contains(fileName))
                throw new InvalidOperationException($"File {fileName} already exists.");

            base.Apply(new RecipePictureAttached(fileName));
        }

        private void OnCreated(RecipeCreated e)
        {
            base.ID = e.RecipeID;
            this.Title = e.Title;
            this.Description = e.Description;
            this.Instructions = e.Instructions;
            this.Ingredients = e.Ingredients;
            this.Servings = e.Servings;
            this.CreatedOn = e.CreatedOn;
        }

        private void OnUpdated(RecipeUpdated e)
        {
            this.Title = e.Title;
            this.Description = e.Description;
            this.Instructions = e.Instructions;
            this.Ingredients = e.Ingredients;
            this.Servings = e.Servings;
        }

        private void OnFavourited(RecipeFavourited e)
        {
            this.IsFavourite = e.IsFavourite;
        }

        private void OnPictureAttached(RecipePictureAttached e)
        {
            (this.PictureFileNames as List<string>).Add(e.FileName);
        }
    }
}
