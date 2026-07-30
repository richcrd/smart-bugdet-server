namespace SMB.APPLICATION.DTOs.Analytics;

public class AnalyticsResponseDto
{
    public decimal CurrentBalance { get; set; }
    public decimal TotalIncomeMonth { get; set; }
    public decimal TotalExpenseMonth { get; set; }
    public string CurrencyCode { get; set; } = "";
    public string CurrencySymbol { get; set; } = "";
    public List<MonthlyTrendDto> MonthlyTrend { get; set; } = [];
}

public class MonthlyTrendDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal Expenses { get; set; }
    public decimal Incomes { get; set; }
}
