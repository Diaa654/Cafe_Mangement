using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.CommonResult
{
    public class Result
    {
        protected readonly List<Error> _errors = [];
        public bool IsSuccess => _errors.Count == 0;

        public IReadOnlyList<Error> Errors => _errors;

        //OK
        protected Result()
        {

        }

        //fail with error
        protected Result(Error error)
        {
            _errors.Add(error);
        }
        //fail with errors 
        protected Result(List<Error> errors)
        {
            _errors.AddRange(errors);
        }

        public static Result Ok() => new Result();

        public static Result Fail(Error error) => new Result(error);

        public static Result Fail(List<Error> errors) => new Result(errors);

        public static implicit operator Result(Error error) => Fail(error);

        public static implicit operator Result(List<Error> errors) => Fail(errors);

    }

    public class Result<TValue> : Result
    {
        private readonly TValue _value;
        public TValue Value => IsSuccess ? _value : throw new InvalidOperationException();

        //Ok With Value
        private Result(TValue value) : base()
        {
            _value = value;
        }
        // fail with error 
        private Result(Error error) : base(error)
        {
            _value = default!;
        }
        //fail with errors
        private Result(List<Error> errors) : base(errors)
        {
            _value = default!;
        }

        public static Result<TValue> Ok(TValue value) => new Result<TValue>(value);
        public static new Result<TValue> Fail(Error error) => new Result<TValue>(error);
        public static new Result<TValue> Fail(List<Error> errors) => new Result<TValue>(errors);


        public static implicit operator Result<TValue>(TValue value) => Ok(value);

        public static implicit operator Result<TValue>(Error error) => Fail(error);

        public static implicit operator Result<TValue>(List<Error> errors) => Fail(errors);
    }
}
