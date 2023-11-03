using ShoppingListApi.Models;

namespace ShoppingListApi.Interfaces
{
    public interface IShoppingListProvider
    {
        Task<List<ShoppingList>> GetShoppingList(string userId);

        Task<string> AddItem(ShoppingList item);

        Task<string> EditItem(ShoppingList item);

        Task<string> DeleteItem(int index);
    }
}