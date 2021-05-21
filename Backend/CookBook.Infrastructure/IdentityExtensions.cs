using System;

namespace CookBook.Infrastructure
{
    public static class IdentityExtensions
    {
        public static string GetDocumentID(this object o, Guid id) => $"{o.GetType().Name}-{id}";
        public static string GetDocumentID<T>(Guid id) => $"{typeof(T).Name}-{id}";
    }
}