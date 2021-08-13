namespace Plastic
{
    public record ExecutionResult<T> : ExecutionResult
    {
        public readonly T? Value;

        public T RequiredValue
            => this.Value!;

        internal protected ExecutionResult(T value)
            : this(true, default, value)
        {
        }

        internal protected ExecutionResult(bool success, string? message, T? value = default)
            : this(new Response(success, message), value)
        {
        }

        internal protected ExecutionResult(Response response, T? value = default)
            : base(response)
        {
            this.Value = value;
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
