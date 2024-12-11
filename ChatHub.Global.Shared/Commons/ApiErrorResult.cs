namespace ChatHub.Global.Shared.Commons
{
    public class ApiErrorResult<T> : ApiResult<T>
    {
        public string[]? ValidationErrors { get; set; }

        public ApiErrorResult(string message)
        {
            IsSuccess = false;
            Message = message;
        }

        public ApiErrorResult(string[] validationErrors)
        {
            IsSuccess = false;
            ValidationErrors = validationErrors;
        }
    }
}
