using AuthService.API.Models;

namespace AuthService.API.Repositories.Interfaces
{
    public interface IRefreshTokensRepository
    {
        Task SaveToken(RefreshToken refreshToken);
    }
}