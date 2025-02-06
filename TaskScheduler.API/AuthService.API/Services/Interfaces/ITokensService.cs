using AuthService.API.Entities;

namespace AuthService.API.Services.Interfaces
{
    public interface ITokensService
    {
        string GenerateAccessToken(UserEntity userEntity);
        string GenerateRefreshToken(UserEntity userEntity);
    }
}