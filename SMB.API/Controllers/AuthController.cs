using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMB.API.Contracts;
using SMB.APPLICATION.DTOs.Auth;
using SMB.APPLICATION.Interfaces.Services;

namespace SMB.API.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IAuthService service) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        var result = await service.Register(request);

        var response = new Answer<RegisterUserResponse>
        {
            Message = "Usuario registrado correctamente",
            Response = result,
            Code = StatusCodes.Status201Created
        };

        return StatusCode(StatusCodes.Status201Created, response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await service.Login(request);

        var response = new Answer<LoginResponse>
        {
            Message = "Inicio de sesión exitoso",
            Response = result,
            Code = StatusCodes.Status200OK
        };

        return Ok(response);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        var result = await service.Refresh(request);

        var response = new Answer<LoginResponse>()
        {
            Message = "Token renovado correctamente",
            Response = result,
            Code = StatusCodes.Status200OK
        };

        return Ok(response);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await service.Logout(request.RefreshToken, userId);

        return Ok(new Answer<object?>
        {
            Message = "Sesión cerrada correctamente",
            Response = null,
            Code = StatusCodes.Status200OK
        });
    }

    [Authorize]
    [HttpPost("logout-all")]
    public async Task<IActionResult> LogoutAll()
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await service.LogoutAll(userId);

        return Ok(new Answer<object?>
        {
            Message = "Todas las sesiones fueron cerradas",
            Response = null,
            Code = StatusCodes.Status200OK
        });
    }
}