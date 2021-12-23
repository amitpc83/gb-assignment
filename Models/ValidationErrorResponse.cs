namespace GlobalBlue.Assignment
{
    public class ValidationErrorResponse:ResponseBase
    {
        public IEnumerable<Error>? Errors { get; set; }
    }

    public class Error
    {
        public string? ErrorType { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
