namespace ShopeeFoodClone.WebMvc.Administrators.Application.Models.ViewModels;

public sealed class UpdateStoreViewModel
{
    public Guid StoreId { get; set; }
    public string? Name { get; set; }
    public string? WardCode { get; set; }
    public string? DistrictCode { get; set; }
    public string? ProvinceCode { get; set; }
    public string? StreetName { get; set; }
    public TimeOnly OpeningHour { get; set; }
    public TimeOnly ClosingHour { get; set; }
    public string? CoverImagePath { get; set; }
    public Guid ConcurrencyStamp { get; set; }
    public IFormFile? CoverImage { get; set; }
}
