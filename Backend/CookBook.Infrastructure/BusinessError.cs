using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.Infrastructure
{
    public class BusinessError
    {
        public string? PropertyName { get; private set; }
        public string Message { get; private set; }

        public BusinessError(string message)
        {
            this.Message = message;
        }

        public BusinessError(string? propertyName, string message)
            : this(message)
        {
            this.PropertyName = propertyName;
        }
    }
}
