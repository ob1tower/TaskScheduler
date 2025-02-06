using AuthService.API.Dtos;
using AuthService.API.Models;
using AuthService.API.Repositories.Interfaces;
using AuthService.API.Services.Interfaces;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;

namespace AuthService.API.Services;

public class UsersService : IUsersService
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUsersRepository _usersRepository;
    private readonly ITokensService _tokensService;
    private readonly IRefreshTokensRepository _refreshtokensRepository;
    private readonly JwtOptions _jwtOptions;
    private readonly IRabbitMqService _rabbitMqService;
    private readonly ILogger<UsersService> _logger;

    public UsersService(IPasswordHasher passwordHasher, IUsersRepository usersRepository,
                       ITokensService tokensService, IRefreshTokensRepository refreshtokensRepository,
                       IOptions<JwtOptions> options, IRabbitMqService rabbitMqService, ILogger<UsersService> logger)
    {
        _passwordHasher = passwordHasher;
        _usersRepository = usersRepository;
        _tokensService = tokensService;
        _refreshtokensRepository = refreshtokensRepository;
        _jwtOptions = options.Value;
        _rabbitMqService = rabbitMqService;
        _logger = logger;
    }

    public async Task<Result> Register(UserRegisterDto userRegisterDto)
    {
        _logger.LogInformation("Запрос на регистрацию пользователя.");

        var userName = await _usersRepository.GetByUserName(userRegisterDto.UserName);

        if (userName != null)
        {
            _logger.LogWarning("Имя пользователя уже существует.");
            return Result.Failure("Имя пользователя уже существует.");
        }

        var email = await _usersRepository.GetByEmail(userRegisterDto.Email);

        if (email != null)
        {
            _logger.LogWarning("Почта уже существует.");
            return Result.Failure("Почта уже существует.");
        }

        var hashpassword = _passwordHasher.Generate(userRegisterDto.Password);

        var user = new User(Guid.NewGuid(), userRegisterDto.UserName,
                            userRegisterDto.Email.ToLower().Normalize(), hashpassword);

        await _usersRepository.CreateUsers(user);

        var message = new
        {
            UserId = user.UserId.ToString(),
            EventType = "register",
        };

        _rabbitMqService.PublishMessage("NoteService", message);

        _logger.LogInformation("Пользователь зарегистрировался.");

        return Result.Success();
    }

    public async Task<Result<TokenDto>> Login(UserLoginDto userLoginDto)
    {
        _logger.LogInformation("Запрос на авторизацию пользователя.");

        var email = await _usersRepository.GetByEmail(userLoginDto.Email);

        if (email == null)
        {
            _logger.LogWarning("Неправильная почта.");
            return Result.Failure<TokenDto>("Неправильная почта.");
        }

        var user = await _usersRepository.GetByEmail(userLoginDto.Email);

        var password = _passwordHasher.Verify(userLoginDto.Password, user.PasswordHash);

        if (!password)
        {
            _logger.LogWarning("Неверный пароль.");
            return Result.Failure<TokenDto>("Неверный пароль.");
        }

        var accessToken = _tokensService.GenerateAccessToken(user);
        var refreshToken = _tokensService.GenerateRefreshToken(user);

        var token = new RefreshToken(refreshToken, DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpires))
        {
            RefreshTokenId = Guid.NewGuid(),
            UserId = user.UserId
        };

        await _refreshtokensRepository.SaveToken(token);

        var tokenDto = new TokenDto(accessToken, refreshToken);

        _logger.LogInformation("Пользователь авторизовался.");

        return Result.Success(tokenDto);
    }
}
