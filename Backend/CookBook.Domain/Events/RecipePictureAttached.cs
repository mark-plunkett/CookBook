using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.Domain.Events
{
    public record RecipePictureAttached(string FileName) : IDomainEvent
    { }
}
