namespace StudyBuddy.Common
{
    public class ErrorResponse : Exception
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ErrorResponse(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}
