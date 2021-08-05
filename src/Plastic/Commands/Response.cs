namespace Plastic
{
    public record Response
    {
        public readonly ResponseState State;

        internal protected Response(bool success, string? message)
            : this(new ResponseState(success, message))
        {
        }

        internal protected Response(ResponseState state)
        {
            this.State = state;
        }

        public bool HasSucceed()
        {
            return this.State.Success;
        }
    }
}
