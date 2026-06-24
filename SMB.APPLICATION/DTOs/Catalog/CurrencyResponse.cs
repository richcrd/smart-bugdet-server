namespace SMB.APPLICATION.DTOs.Catalog;

public class CurrencyResponse
{
    public long Id { get; set; }
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string Symbol { get; set; } = "";
    public int DecimalPlaces { get; set; }
}