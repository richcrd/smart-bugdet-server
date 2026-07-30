namespace SMB.APPLICATION.DTOs.Catalog;

public class SubcategoriesResponse
{
    public long Id { get; set; }
    public string? Name { get; set; } = "";
    public string? Icon { get; set; } = "";
    public long UserId { get; set; }
    public bool IsSystem { get; set; }
}