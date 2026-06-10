namespace MiniShopping.Web.Models
{
    public class BasketItem
    {
        public int Id { get; set; }
        public string UserId { get; set; }   // IdentityUser → string
        public ApplicationUser? User { get; set; } = null;
        public int ProductId { get; set; }
        public Product? Product { get; set; } = null;
        public int Quantity { get; set; }
    }
}