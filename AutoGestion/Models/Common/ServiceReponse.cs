namespace AutoGestion.Models.Common
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public static ServiceResponse<T> Ok(T data, string message = "Operación exitosa")
        {
            return new ServiceResponse<T> { Success = true, Data = data, Message = message };
        }

        public static ServiceResponse<T> Fail(string errorMessage)
        {
            return new ServiceResponse<T> { Success = false, Message = errorMessage, Data = default };
        }
    }
}
