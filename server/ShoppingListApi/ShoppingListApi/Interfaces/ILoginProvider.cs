using ShoppingListApi.Models;

namespace ShoppingListApi.Interfaces
{
    public interface ILoginProvider
    {
        Task<ApiResponse> LoginWithFacebook(string credentials);
    }
}