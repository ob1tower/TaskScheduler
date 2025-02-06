using AuthService.API.Dtos;
using CSharpFunctionalExtensions;

namespace AuthService.API.Services.Interfaces
{
    public interface IUsersService
    {
        Task<Result<TokenDto>> Login(UserLoginDto userLoginDto);
        Task<Result> Register(UserRegisterDto userRegisterDto);
    }
}