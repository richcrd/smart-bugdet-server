using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using SMB.API.Contracts;

namespace SMB.API.Controllers;

[Authorize]
[ApiController]
[Route("user")]
public class UserController : ControllerBase
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
}