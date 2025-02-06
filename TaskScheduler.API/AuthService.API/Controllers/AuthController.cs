using AuthService.API.Dtos;
using AuthService.API.Error;
using AuthService.API.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUsersService _usersService;
    private readonly IValidator<UserRegisterDto> _registerUserValidator;
    private readonly IValidator<UserLoginDto> _loginUserValidator;

    public AuthController(IUsersService usersService, IValidator<UserRegisterDto> registerUserValidator, IValidator<UserLoginDto> loginUserValidator)
    {
        _usersService = usersService;
        _registerUserValidator = registerUserValidator;
        _loginUserValidator = loginUserValidator;
    }

    /// <summary>
    /// Регистрация
    /// </summary>
    /// <param name="userRegisterDto">Данные пользователя для регистрации</param>
    /// <returns></returns>
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
    {
        var validationResult = await _registerUserValidator.ValidateAsync(userRegisterDto);

        if (!validationResult.IsValid)
            return BadRequest(ErrorFormat.Deserialize(validationResult.Errors));

        var result = await _usersService.Register(userRegisterDto);

        if (result.IsFailure)
            return Conflict(new { message = result.Error });

        return Ok(new { message = "Пользователь успешно зарегистрирован." });
    }

    /// <summary>
    /// Авторизация
    /// </summary>
    /// <param name="userLoginDto">Данные пользователя для авторизации</param>
    /// <returns></returns>
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
    {
        var validationResult = await _loginUserValidator.ValidateAsync(userLoginDto);

        if (!validationResult.IsValid)
            return BadRequest(ErrorFormat.Deserialize(validationResult.Errors));

        var result = await _usersService.Login(userLoginDto);

        if (result.IsFailure)
            return Unauthorized(new { message = result.Error });

        return Ok(new { message = "Пользователь успешно авторизован.", tokens = result.Value });
    }
}
