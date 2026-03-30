namespace PremierLeague_Api.Responses
{
    public class Response<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Contents { get; set; }
        public bool IsSuccess { get; set; }

        public Response(int statusCode, string message, T contents, bool isSuccess)
        {
            StatusCode = statusCode;
            Message = message;
            Contents = contents;
            IsSuccess = isSuccess;
        }
    }
}