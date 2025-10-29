using jwtAuthentication.Entities;
using jwtAuthentication.Models;

namespace jwtAuthentication.Services
{
    public interface IAuthService
    {
        Task<TokenResponseDto?> LoginAsync(UserDto request);
        Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request);
        Task<User?> RegisterAsync(UserDto request);
    }
}
