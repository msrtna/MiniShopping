using MiniShopping.Web.DTOs.AccountDtos;

namespace MiniShopping.Web.Services.AccountServices
{
    public interface IAccountService
    {
        Task<string> RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto);
    }
}
