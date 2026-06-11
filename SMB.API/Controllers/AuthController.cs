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
}