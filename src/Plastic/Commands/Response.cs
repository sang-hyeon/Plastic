namespace Plastic
{
    public record ExecutionResult
    {
        public readonly Response Response;

        internal protected ExecutionResult()
            : this(true, default)
        {
        }

        internal protected ExecutionResult(bool success, string? message)
           : this(new Response(success, message))
        {
        }

        internal protected ExecutionResult(Response state)
        {
            this.Response = state;
        }

        public bool HasSucceed()
        {
            return this.Response.Result;
        }
    }
}
