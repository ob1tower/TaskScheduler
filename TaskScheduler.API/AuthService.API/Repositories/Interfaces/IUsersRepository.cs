using AuthService.API.Entities;
using AuthService.API.Models;

namespace AuthService.API.Services.Interfaces
{
    public interface IUsersRepository
    {
        Task CreateUsers(User user);
        Task<UserEntity?> GetByEmail(string email);
        Task<UserEntity?> GetByUserName(string userName);
    }
}