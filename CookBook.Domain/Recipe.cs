using CookBook.Domain.Commands;
using CookBook.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.Domain
{
    public class Recipe : Aggregate
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Instructions { get; private set; }
        public string Ingredients { get; private set; }
        public int Servings { get; private set; }
        public bool IsFavourite { get; private set; }

        protected override void When(IEvent @event)
        {
            switch (@event)
            {
                case RecipeCreated e: OnCreated(e); break;
            }
        }

        public void Create(CreateRecipeCommand command)
        {
            if (base.Version >= 0)
                throw new InvalidOperationException("Already created");

            if (string.IsNullOrWhiteSpace(command.Title))
                throw new ArgumentNullException(nameof(command.Title));

            if (command.Servings < 1)
                throw new ArgumentOutOfRangeException(nameof(command.Servings));

            base.Apply(new RecipeCreated(
                Guid.NewGuid(),
                command.Title,
                command.Description,
                command.Instructions,
                command.Ingredients,
                command.Servings));
        }

        private void OnCreated(RecipeCreated e)
        {
            base.ID = e.RecipeID;
            this.Title = e.Title;
            this.Description = e.Description;
            this.Instructions = e.Instructions;
            this.Ingredients = e.Ingredients;
            this.Servings = e.Servings;
        }
    }
}
