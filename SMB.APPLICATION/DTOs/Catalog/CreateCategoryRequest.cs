namespace SMB.APPLICATION.DTOs.Catalog;

public class CreateCategoryRequest
{
    public string? Name { get; set; }
    public string? Icon { get; set; }
    public string? Color { get; set; }
    public long TransactionTypeId { get; set; }
}