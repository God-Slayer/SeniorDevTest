namespace ShoppingListApi.Models
{
    public class ShoppingList
    {
        public int? Index { get; set; }

        public string? UserId { get; set; }

        public string? Item { get; set; }

        public int? Quantity { get; set; }

        public IFormFile? Image { get; set; }
    }
}