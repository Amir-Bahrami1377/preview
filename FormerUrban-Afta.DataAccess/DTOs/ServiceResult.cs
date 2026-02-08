namespace FormerUrban_Afta.DataAccess.DTOs
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ServiceResult<T> IsSuccess(T data, string message = null)
        {
            Success = true;
            Data = data;
            Message = message;
            return this;
        }

        public ServiceResult<T> IsFailed(string message, T data)
        {
            Success = false;
            Data = data;
            Message = message;
            return this;
        }
    }
}
