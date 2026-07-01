namespace SMB.APPLICATION.DTOs.Summary;

public class DashboardSummaryDto
{
    public decimal CurrentBalance { get; set; }
    public decimal TotalIncomeMonth { get; set; }
    public decimal TotalExpenseMonth { get; set; }
    public string CurrencyCode { get; set; } = "";
    public string CurrencySymbol { get; set; } = "";
}