namespace ShopeeFoodClone.WebMvc.Administrators.Application.Models.ViewModels;

public sealed class CreateStoreViewModel
{
    public Guid VendorId { get; set; }
    [Required(ErrorMessage = "Tên cửa hàng không được bỏ trống")]
    [MinLength(10, ErrorMessage = "Tên phải chứa ít nhất 10 ký tự")]
    [MaxLength(50, ErrorMessage = "Tên không được vượt quá 50 ký tự")]
    [RegularExpression("^[a-zA-Z0-9-]+$", ErrorMessage = "Tên không được chứa ký tự đặc biệt")]
    public string? Name { get; set; }
    [RegularExpression("^[a-zA-Z0-9-]+$", ErrorMessage = "Địa chỉ không được chứa ký tự đặc biệt")]
    public string? WardCode { get; set; }
    public string? StreetName { get; set; }
    public TimeOnly OpeningHour { get; set; }
    public TimeOnly ClosingHour { get; set; }
    public List<Guid> SubCategoryIds { get; set; } = new List<Guid>();
}
