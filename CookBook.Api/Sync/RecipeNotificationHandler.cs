using CookBook.Domain;
using CookBook.Domain.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CookBook.Api.Sync
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
            switch (notification.Event)
            {
                case RecipeCreated: return this.hubContext.Clients.All.RecipeCreated(notification.Recipe);
                default: return this.hubContext.Clients.All.RecipeUpdated(notification.Recipe);
            }
        }
    }
}
