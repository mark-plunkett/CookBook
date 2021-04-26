using CookBook.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookBook.Api
{
    public static class ModelStateExtensions
    {
        public static ModelStateDictionary WithBusinessErrors(this ModelStateDictionary modelState, IEnumerable<BusinessError> errors)
        {
            foreach (var error in errors)
            {
                modelState.AddModelError(error.PropertyName?.ToCamelCase() ?? string.Empty, error.Message);
            }

            return modelState;
        }

        public static string ToCamelCase(this string s)
        {
            return $"{s.Substring(0, 1).ToLower()}{s.Substring(1)}";
        }
    }
}
