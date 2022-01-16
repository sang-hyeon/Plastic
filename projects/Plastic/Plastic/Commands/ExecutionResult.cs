namespace Plastic
{
    using System;

    public record ExecutionResult<T> : ExecutionResult
    {
        private readonly object? _value;

        public T RequiredValue
            => (T)this._value!;

        internal protected ExecutionResult(T value)
            : this(true, default, value)
        {
        }

        internal protected ExecutionResult(bool success, string? message, object? value = default)
            : this(new Response(success, message), value)
        {
        }

        internal protected ExecutionResult(Response response, object? value = default)
            : base(response)
        {
            if (value is not null & value is not T)
                throw new ArgumentException($"value is not {typeof(T)}");

            this._value = value;
        }
    }

    public record ExecutionResult : Response
    {
        internal protected ExecutionResult(bool success, string? message = default)
            : base(success, message)
        {
        }

        internal protected ExecutionResult(Response response)
            : base(response.Result, response.Message)
        {
        }

        public bool HasSucceeded()
        {
            return this.Result;
        }
    }
}
