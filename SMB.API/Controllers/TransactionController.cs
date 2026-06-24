using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace SMB.API.Controllers;

[ApiController]
[Route("transaction")]
public class TransactionController() : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create()
    {
        return Ok();
    }
}