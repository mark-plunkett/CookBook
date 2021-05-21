using System.Collections.Generic;
using System.Threading.Tasks;

namespace CookBook.Domain.Tags
{
    public interface ITagRepo
    {
        Task<IEnumerable<Tag>> CreateTags(IEnumerable<string> tagNames);
    }
}