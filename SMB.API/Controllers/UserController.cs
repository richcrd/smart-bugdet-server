using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMB.API.Contracts;
using SMB.APPLICATION.DTOs.User;
using SMB.APPLICATION.Interfaces.Services;

namespace SMB.API.Controllers;

[Authorize]
[ApiController]
[Route("user")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet("me")]
    public IActionResult Me()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userName = User.Identity?.Name;

        var result = new Answer<object>
        {
            Message = "Usuario autenticado",
            Response = new
            {
                UserId = userId,
                UserName = userName
            },
            Code = StatusCodes.Status200OK
        };

        return Ok(result);
    }

    [HttpPatch("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await userService.UpdateProfile(userId, request);

        return Ok(new Answer<object?>
        {
            Message = "Perfil actualizado correctamente",
            Response = null,
            Code = StatusCodes.Status200OK
        });
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await userService.ChangePassword(userId, request);

        return Ok(new Answer<object?>
        {
            Message = "Contraseña actualizada correctamente",
            Response = null,
            Code = StatusCodes.Status200OK
        });
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var profile = await userService.GetProfile(userId);

        return Ok(new Answer<UserProfileResponse>
        {
            Message = "Perfil obtenido correctamente",
            Response = profile,
            Code = StatusCodes.Status200OK
        });
    }
}