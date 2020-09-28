using System.Collections.Generic;
using System.Linq;

namespace PayMeWithRocks.Application.Common.Models
{
    public class Result<T>
    {
        internal Result(bool succeeded, T value, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            Errors = errors.ToArray();
            Value = value;
        }

        public bool Succeeded { get; set; }

        public string[] Errors { get; set; }

        public T Value { get; set; }

        public static Result<T> Success(T value)
        {
            return new Result<T>(true, value, new string[] { });
        }

        public static Result<T> Failure(IEnumerable<string> errors)
        {
            return new Result<T>(false, default, errors);
        }
    }
}