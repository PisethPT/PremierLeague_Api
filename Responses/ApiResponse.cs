namespace PremierLeague_Api.Responses
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Contents { get; set; }

        public ApiResponse(int statusCode, string message, T contents)
        {
            StatusCode = statusCode;
            Message = message;
            Contents = contents;
        }
    }
}
