namespace MiniShopping.Web.DTOs.BasketItemDtos
{
    public class UpdateBasketItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
