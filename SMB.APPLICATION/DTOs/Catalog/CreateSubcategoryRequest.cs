namespace SMB.APPLICATION.DTOs.Catalog;

public class CreateSubcategoryRequest
{
    public long CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public string? Icon { get; set; }
}