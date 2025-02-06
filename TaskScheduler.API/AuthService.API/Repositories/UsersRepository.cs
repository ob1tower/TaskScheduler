using AuthService.API.DataAccess;
using AuthService.API.Entities;
using AuthService.API.Models;
using AuthService.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthService.API.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly UserDbContext _context;

    public UsersRepository(UserDbContext context)
    {
        _context = context;
    }

    public async Task CreateUsers(User user)
    {
        var userEntities = new UserEntity
        {
            UserId = user.UserId,
            UserName = user.UserName,
            PasswordHash = user.PasswordHash,
            Email = user.Email,
            CreatedAt = DateTime.UtcNow,
            RoleId = 1
        };

        await _context.Users.AddAsync(userEntities);
        await _context.SaveChangesAsync();
    }

    public async Task<UserEntity?> GetByEmail(string email)
    {
        return await _context.Users
            .AsTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<UserEntity?> GetByUserName(string userName)
    {
        return await _context.Users
            .AsTracking()
            .FirstOrDefaultAsync(u => u.UserName == userName);
    }
}
