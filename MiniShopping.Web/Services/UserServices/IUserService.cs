using MiniShopping.Web.DTOs.UserDtos;

namespace MiniShopping.Web.Services.UserServices
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsersAsync();
    }
}
