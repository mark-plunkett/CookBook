using CookBook.Model.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.Model
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

        public void Create(
            string title,
            string description,
            string instructions,
            string ingredients,
            int servings)
        {
            if (base.Version >= 0)
                throw new InvalidOperationException("Already created");

            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentNullException(nameof(title));

            base.Apply(new RecipeCreated(base.ID, title, description, instructions, ingredients, servings));
        }

        private void OnCreated(RecipeCreated e)
        {
            this.Title = e.Title;
            this.Description = e.Description;
            this.Instructions = e.Instructions;
            this.Ingredients = e.Ingredients;
            this.Servings = e.Servings;
        }
    }
}
