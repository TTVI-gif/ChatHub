namespace ChatHub.Global.Shared.Commons
{
    public class ApiResult<T>
    {
        public string? Message { get; set; }
        public bool IsSuccess { get; set; }
        public T? ResultObj { get; set; }
    }
}
