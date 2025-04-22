namespace ShopeeFoodClone.WebMvc.Administrators.Application.Models.ViewModels;

public sealed class CreateProductViewModel
{
    public Guid StoreId { get; set; }
    public string? Name { get; set; }

    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public IFormFile? CoverImage { get; set; }
}
