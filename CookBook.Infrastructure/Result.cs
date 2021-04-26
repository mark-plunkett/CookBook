using System;
using System.Collections;
using System.Collections.Generic;

namespace CookBook.Infrastructure
{
    public class Result<TSuccess>
    {
        public bool IsSuccess => this is Success;

        private Result()
        { }

        public T Handle<T>(Func<TSuccess, T> success, Func<IEnumerable<BusinessError>, T> error)
        {
            if (this is Success s)
                return success.Invoke(s.result);
            else if (this is Error e)
                return error.Invoke(e.errors);
            else
                throw new NotSupportedException();
        }

        public Result<TSuccess> Combine(Result<TSuccess> other)
        {
            if (this.IsSuccess && other.IsSuccess)
                return this;

            if (!this.IsSuccess && other.IsSuccess)
                return this;

            if (this.IsSuccess && !other.IsSuccess)
                return other;

            if (this is Error thisError && other is Error otherError)
            {
                foreach (var error in otherError.errors)
                {
                    thisError.AddError(error);
                }

                return thisError;
            }

            throw new NotSupportedException();
        }

        public class Success : Result<TSuccess>
        {
            internal readonly TSuccess result;

            public Success(TSuccess result)
            {
                this.result = result;
            }


        }

        public class Error : Result<TSuccess>
        {
            internal readonly List<BusinessError> errors = new List<BusinessError>();

            public Error(BusinessError error)
            {
                this.errors.Add(error);
            }

            public Error(string message)
                : this(new BusinessError(message))
            { }

            public Error(string propertyName, string message)
                : this(new BusinessError(propertyName, message))
            { }

            internal void AddError(BusinessError error)
            {
                this.errors.Add(error);
            }
        }
    }
}
