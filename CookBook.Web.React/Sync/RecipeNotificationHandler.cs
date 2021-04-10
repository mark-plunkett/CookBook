using CookBook.Domain;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CookBook.Web.React.Sync
{
    public class RecipeNotificationHandler : INotificationHandler<RecipeModifiedNotification>
    {
        private readonly IHubContext<RecipeHub, IRecipeClient> hubContext;

        public RecipeNotificationHandler(
            IHubContext<RecipeHub, IRecipeClient> hubContext)
        {
            this.hubContext = hubContext;
        }

        public Task Handle(RecipeModifiedNotification notification, CancellationToken cancellationToken)
        {
            return this.hubContext.Clients.All.RecieveRecipe(notification.Recipe);
        }
    }
}
