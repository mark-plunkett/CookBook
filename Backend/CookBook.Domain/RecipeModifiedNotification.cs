using CookBook.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.Domain
{
    public record RecipeModifiedNotification(
        Recipe Recipe,
        IDomainEvent Event
        ) : INotification
    { }
}
