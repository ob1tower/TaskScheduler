using AuthService.API.DataAccess;
using AuthService.API.Entities;
using AuthService.API.Models;
using AuthService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthService.API.Repositories;

public class RefreshTokensRepository : IRefreshTokensRepository
{
    private readonly UserDbContext _context;

    public RefreshTokensRepository(UserDbContext context)
    {
        _context = context;
    }

    public async Task SaveToken(RefreshToken refreshToken)
    {
        var refreshTokenEntities = new RefreshTokenEntity
        {
            RefreshTokenId = refreshToken.RefreshTokenId,
            Token = refreshToken.Token,
            CreatedAt = DateTime.UtcNow,
            Expires = refreshToken.Expires,
            UserId = refreshToken.UserId
        };

        await _context.RefreshTokens.AddAsync(refreshTokenEntities);
        await _context.SaveChangesAsync();
    }
}
