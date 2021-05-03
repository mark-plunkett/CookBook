using MediatR;

namespace CookBook.Domain.Tags.Commands
{
    public class CreateTagCommand : IRequest
    {
        public string Name { get; set; }
    }
}
