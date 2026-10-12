namespace TrafficFineManagement.Web.Models.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public Result(bool isSuccess, T data, string message = "")
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }

        public static Result<T> Success(T data)
        {
            return new Result<T>(true, data);
        }

        public static Result<T> Failure(string message, T data = default!)
        {
            return new Result<T>(false, default!, message);
        }
    }
}
