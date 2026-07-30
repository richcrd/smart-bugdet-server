using SMB.APPLICATION.DTOs.Expenses;

namespace SMB.APPLICATION.Interfaces.Services;

public interface IExpensesService
{
    Task<ExpensesResponseDto> GetExpensesByMonth(long userId, int year, int month);
}
