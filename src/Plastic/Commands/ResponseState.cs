namespace Plastic
{
    public record Response
    {
        public readonly bool Result;

        public readonly string? Message;

        internal Response(bool result = true, string? message = default)
        {
            this.Result = result;
            this.Message = message;
        }
    }
}
