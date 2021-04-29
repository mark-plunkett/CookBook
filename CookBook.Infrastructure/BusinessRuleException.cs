using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CookBook.Infrastructure
{
    public class BusinessRuleException : Exception
    {
        public IEnumerable<BusinessError> Errors { get; private set; }

        public BusinessRuleException(BusinessError error)
        {
            this.Errors = new [] { error };
        }

        public BusinessRuleException(IEnumerable<BusinessError> errors)
        {
            this.Errors = errors;
        }
    }
}