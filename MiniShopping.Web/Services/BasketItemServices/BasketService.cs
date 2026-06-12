using MiniShopping.Web.DTOs.BasketItemDtos;
using MiniShopping.Web.Models;
using MiniShopping.Web.UnitOfWorks;

namespace MiniShopping.Web.Services.BasketItemServices
{
    public class BasketService : IBasketService
    {
        private readonly IUnitOfWork _uow;

        public BasketService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<List<BasketItemDto>> GetBasketItemsAsync(string userId)
        {
            var basket = await _uow.Basket.GetBasketItemsAsync(userId);
            return basket.Select(b => new BasketItemDto
            {
                Id = b.Id,
                UserId = userId,
                ProductId = b.ProductId,
                ProductName = b.Product?.Name ?? "",
                Quantity = b.Quantity,
                UnitPrice = b.Product?.Price ?? 0,
                TotalPrice = b.Quantity * (b.Product?.Price ?? 0)
            }).ToList();
        }
        public async Task<string> AddToBasketAsync(CreateBasketItemDto dto)
        {
            if (dto.Quantity <= 0)
                throw new Exception("Quantity must be greater than zero");

            var existingBasket =
                await _uow.Basket.GetByUserAndProductAsync(dto.UserId, dto.ProductId);

            if (existingBasket != null)
            {
                existingBasket.Quantity += dto.Quantity;

                await _uow.Basket.UpdateQuantityAsync(existingBasket);
            }
            else
            {
                var basket = new BasketItem
                {
                    UserId = dto.UserId,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                };

                await _uow.Basket.AddToBasketAsync(basket);
            }

            await _uow.SaveAsync();

            return "Product added to basket successfully";
        }
        public async Task<string> UpdateQuantity(UpdateBasketItemDto dto)
        {
            var basket = await _uow.Basket.GetByIdAsync(dto.Id);
            if (basket == null)
                throw new Exception("Product not found");
            if (dto.Quantity <= 0)
                throw new Exception("Quantity must be up to zero");

            basket.Quantity = dto.Quantity;

            await _uow.Basket.UpdateQuantityAsync(basket);
            await _uow.SaveAsync();

            return "Quantity updated successfuly";
        }
        public async Task<string> DeleteBasketAsync(int id, string userId)
        {
            var basketUser = await _uow.Basket.GetBasketItemsAsync(userId);
            if (!basketUser.Any())
                throw new Exception("Basket not found");

            var basket = basketUser.FirstOrDefault(b => b.Id == id);
            if (basket == null)
                throw new Exception("Basket not found");

            await _uow.Basket.DeleteBasketAsync(basket.Id, basket.UserId);
            await _uow.SaveAsync();

            return "Basket deleted successfuly";
        }
        public async Task<BasketItemDto?> GetByUserAndProductAsync(string userId, int productId)
        {
            var basket = await _uow.Basket.GetByUserAndProductAsync(userId, productId);
            if (basket == null)
                throw new Exception("Basket not found");
            return new BasketItemDto
            {
                Id = basket.Id,
                UserId = userId,
                ProductId = basket.ProductId,
                ProductName = basket.Product?.Name ?? "",
                Quantity = basket.Quantity,
                UnitPrice = basket.Product?.Price ?? 0,
            };
        }
    }
}
