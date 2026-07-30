using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMB.API.Contracts;
using SMB.APPLICATION.DTOs.Expenses;
using SMB.APPLICATION.Interfaces.Services;

namespace SMB.API.Controllers;

[ApiController]
[Route("expenses")]
[Authorize]
public class ExpensesController(IExpensesService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetExpenses([FromQuery] int? year, [FromQuery] int? month)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var now = DateTime.UtcNow;
        var expenses = await service.GetExpensesByMonth(
            userId,
            year ?? now.Year,
            month ?? now.Month);

        var result = new Answer<ExpensesResponseDto>
        {
            Message = "Gastos obtenidos correctamente",
            Response = expenses,
            Code = StatusCodes.Status200OK
        };

        return Ok(result);
    }
}
