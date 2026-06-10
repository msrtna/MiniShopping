using MiniShopping.Web.Models;

namespace MiniShopping.Web.DTOs.BasketItemDtos
{
    public class CreateBasketItemDto
    {
        public string UserId { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
