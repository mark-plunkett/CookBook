using System.Collections.Generic;
using System.Linq;
using CookBook.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Api
{
    internal class BusinessRuleExceptionProblemDetails : ProblemDetails
    {
        public IEnumerable<BusinessError> businessErrors { get; }

        public BusinessRuleExceptionProblemDetails(BusinessRuleException ex)
        {
            this.businessErrors = ex.Errors.Select(e => new BusinessError(e.PropertyName.ToCamelCase(), e.Message));
            base.Status = 400;
        }
    }
}