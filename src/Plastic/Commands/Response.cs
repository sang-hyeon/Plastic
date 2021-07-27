namespace Plastic
{
    /// <summary>
    /// <see cref="ICommandParameters{TResponse}"/>에 대한 응답입니다.
    /// </summary>
    public record Response
    {
        public readonly ResponseState State;

        /// <summary>
        /// 생성자입니다.
        /// </summary>
        /// <param name="success">성공 여부</param>
        /// <param name="message">응답 상태에 대한 메세지</param>
        internal protected Response(bool success, string? message)
            : this(new ResponseState(success, message))
        {
        }

        /// <summary>
        /// 생성자입니다.
        /// </summary>
        /// <param name="state">응답 상태</param>
        internal protected Response(ResponseState state)
        {
            this.State = state;
        }

        /// <summary>
        /// 성공 여부를 반환합니다.
        /// </summary>
        /// <returns>성공 여부</returns>
        public bool HasSucceed()
        {
            return this.State.Success;
        }
    }
}
