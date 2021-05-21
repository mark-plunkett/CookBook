using System.Collections;
using System.Collections.Generic;
using MediatR;

namespace CookBook.Domain.Tags.Queries
{
    public class ListTagsQueryRequest : IRequest<IEnumerable<Tag>>
    { }
}
