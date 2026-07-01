using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMB.API.Contracts;
using SMB.APPLICATION.DTOs.Wallet;
using SMB.APPLICATION.Interfaces.Services;

namespace SMB.API.Controllers;

[Authorize]
[ApiController]
[Route("wallet")]
public class WalletController(IWalletService walletService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var wallets = await walletService.GetAllByUserId(userId);

        var result = new Answer<List<WalletResponse>>
        {
            Message = "Se ha recuperado la información correctamente",
            Response = wallets,
            Code = StatusCodes.Status200OK
        };

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWalletRequest request)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var wallet = await walletService.Create(userId, request);

        var result = new Answer<WalletResponse>
        {
            Message = "Cartera creada correctamente",
            Response = wallet,
            Code = StatusCodes.Status201Created
        };

        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateWalletRequest request)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var wallet = await walletService.Update(userId, id, request);

        var result = new Answer<WalletResponse>
        {
            Message = "Cartera actualizada correctamente",
            Response = wallet,
            Code = StatusCodes.Status200OK
        };

        return Ok(result);
    }

    [HttpPut("{id}/default")]
    public async Task<IActionResult> SetDefault(long id)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var wallet = await walletService.SetDefault(userId, id);

        var result = new Answer<WalletResponse>
        {
            Message = "Cartera predeterminada actualizada correctamente",
            Response = wallet,
            Code = StatusCodes.Status200OK
        };

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await walletService.Delete(userId, id);

        var result = new Answer<object?>
        {
            Message = "Cartera eliminada correctamente",
            Response = null,
            Code = StatusCodes.Status200OK
        };

        return Ok(result);
    }
}
