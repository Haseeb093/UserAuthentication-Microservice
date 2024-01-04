namespace Domain.CustomModels
{
    public class ResponseObject<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
