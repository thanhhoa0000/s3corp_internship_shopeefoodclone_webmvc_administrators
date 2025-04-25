namespace ShopeeFoodClone.WebMvc.Administrators.Application.Models.ViewModels;

public sealed class UpdateProductViewModel
{
    public Guid ProductId { get; set; }   
    public Guid StoreId { get; set; }
    public string? Name { get; set; }
    public int? AvailableStock { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public decimal? Discount { get; set; }
    public string? CoverImagePath { get; set; }
    public Guid? ConcurrencyStamp { get; set; }
    public IFormFile? CoverImage { get; set; }
}
