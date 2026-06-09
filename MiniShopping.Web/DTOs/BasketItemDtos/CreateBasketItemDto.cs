using MiniShopping.Web.Models;

namespace MiniShopping.Web.DTOs.BasketItemDtos
{
    public class CreateBasketItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
