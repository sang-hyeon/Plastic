namespace Plastic
{
    public record ResponseState
    {
        public readonly bool Success;

        public readonly string? Message;

        internal ResponseState(bool success = true, string? message = default)
        {
            this.Success = success;
            this.Message = message;
        }
    }
}
