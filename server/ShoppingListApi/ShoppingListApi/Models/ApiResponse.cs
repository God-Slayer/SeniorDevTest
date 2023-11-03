namespace ShoppingListApi.Models
{
    public class ApiResponse
    {
        public bool? Success { get; set; }
        public string? Message { get; set; }

        public FBUserInfo? Data { get; set; }

        public ApiResponse()
        {
        }

        public ApiResponse(bool success, string message, FBUserInfo? data = null)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}