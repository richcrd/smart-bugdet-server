namespace SMB.APPLICATION.DTOs.Catalog;

public class CategoriesResponse
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Icon { get; set; }
    public string? Color { get; set; }

    public List<SubcategoriesResponse> Subcategories { get; set; } = [];
}