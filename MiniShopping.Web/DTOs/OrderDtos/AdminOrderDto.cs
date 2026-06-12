namespace MiniShopping.Web.DTOs.OrderDtos
{
    public class AdminOrderDto
    {
        public int Id { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
