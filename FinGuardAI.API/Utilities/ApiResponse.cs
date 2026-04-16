namespace FinGuardAI.API.Utilities
{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }

        public static ApiResponse<T> SuccessResponse(T? data = default, string message = "Success", string code = "SUCCESS")
        {
            return new ApiResponse<T>
            {
                Data = data,
                Message = message,
                Code = code
            };
           
        }

        public static ApiResponse<T> FailureResponse(string message, string code = "ERROR")
        {
            return new ApiResponse<T>
            {
                Message = message,
                Code = code
            };

        }
    }
}
