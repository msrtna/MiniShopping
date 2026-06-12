using AutoMapper;
using MiniShopping.Web.DTOs.BasketItemDtos;
using MiniShopping.Web.Models;
using MiniShopping.Web.UnitOfWorks;

namespace MiniShopping.Web.Services.BasketItemServices
{
    public class BasketService : IBasketService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public BasketService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<List<BasketItemDto>> GetBasketItemsAsync(string userId)
        {
            var basket = await _uow.Basket.GetBasketItemsAsync(userId);

            return _mapper.Map<List<BasketItemDto>>(basket);
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
                var basket = _mapper.Map<BasketItem>(dto);
                await _uow.Basket.AddToBasketAsync(basket);
            }

            await _uow.SaveAsync();

            return "Product added to basket successfully";
        }

        public async Task<string> UpdateQuantity(UpdateBasketItemDto dto)
        {
            var basket = await _uow.Basket.GetByIdAsync(dto.Id);

            if (basket == null)
                throw new Exception("Basket not found");

            if (dto.Quantity <= 0)
                throw new Exception("Quantity must be greater than zero");

            basket.Quantity = dto.Quantity;

            await _uow.Basket.UpdateQuantityAsync(basket);
            await _uow.SaveAsync();

            return "Quantity updated successfully";
        }

        public async Task<string> DeleteBasketAsync(int id, string userId)
        {
            var basket = await _uow.Basket.GetByIdAsync(id);

            if (basket == null || basket.UserId != userId)
                throw new Exception("Basket not found");

            await _uow.Basket.DeleteBasketAsync(basket.Id, basket.UserId);
            await _uow.SaveAsync();

            return "Basket deleted successfully";
        }

        public async Task<BasketItemDto?> GetByUserAndProductAsync(string userId, int productId)
        {
            var basket = await _uow.Basket.GetByUserAndProductAsync(userId, productId);

            if (basket == null)
                throw new Exception("Basket not found");

            return _mapper.Map<BasketItemDto>(basket);
        }
    }
}