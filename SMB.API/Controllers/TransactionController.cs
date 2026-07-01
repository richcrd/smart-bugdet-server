using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMB.API.Contracts;
using SMB.APPLICATION.DTOs.Transaction;
using SMB.APPLICATION.Interfaces.Services;

namespace SMB.API.Controllers;

[ApiController]
[Route("transaction")]
[Authorize]
public class TransactionController(ITransactionService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTransactionRequest request)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var transactionId = await service.Create(request, userId);

        var result = new Answer<long>
        {
            Message  = "Transacción creada correctamente",
            Response = transactionId,
            Code     = StatusCodes.Status201Created
        };

        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] long? walletId)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var transactions = await service.GetAll(userId, walletId);

        var result = new Answer<List<TransactionResponse>>
        {
            Message  = "Transacciones obtenidas correctamente",
            Response = transactions,
            Code     = StatusCodes.Status200OK
        };

        return Ok(result);
    }
}