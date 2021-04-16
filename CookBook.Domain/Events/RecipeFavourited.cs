using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.Domain.Events
{
    public record RecipeFavourited(
        Guid RecipeID,
        bool IsFavourite)
        : IDomainEvent
    {
        //public RecipeFavourited(long version)
        //    : base(version)
        //{
        //    this.rec
        //}
    }
}
