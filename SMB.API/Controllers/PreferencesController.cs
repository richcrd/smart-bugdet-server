using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMB.API.Contracts;
using SMB.APPLICATION.DTOs.Preferences;
using SMB.APPLICATION.Interfaces.Services;

namespace SMB.API.Controllers;

[Authorize]
[ApiController]
[Route("user/preferences")]
public class PreferencesController(IUserPreferenceService preferenceService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var preference = await preferenceService.GetByUserId(userId);

        var result = new Answer<UserPreferenceResponse>
        {
            Message = "Se ha recuperado la información correctamente",
            Response = preference,
            Code = StatusCodes.Status200OK
        };

        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateUserPreferenceRequest request)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var preference = await preferenceService.Update(userId, request);

        var result = new Answer<UserPreferenceResponse>
        {
            Message = "Preferencias actualizadas correctamente",
            Response = preference,
            Code = StatusCodes.Status200OK
        };

        return Ok(result);
    }
}
