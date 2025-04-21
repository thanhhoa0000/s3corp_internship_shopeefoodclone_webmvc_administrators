namespace ShopeeFoodClone.WebMvc.Administrators.Application.Models.ViewModels;

public sealed class CreateProductViewModel
{
    public Guid StoreId { get; set; }
    [Required(ErrorMessage = "Tên sản phẩm không được bỏ trống")]
    [MinLength(10, ErrorMessage = "Tên phải chứa ít nhất 10 ký tự")]
    [MaxLength(50, ErrorMessage = "Tên không được vượt quá 50 ký tự")]
    [RegularExpression("^[a-zA-Z0-9-]+$", ErrorMessage = "Tên không được chứa ký tự đặc biệt")]
    public string? Name { get; set; }
    [Required(ErrorMessage = "Mô tả sản phẩm không được bỏ trống")]
    [MinLength(20, ErrorMessage = "Mô tả phải chứa ít nhất 20 ký tự")]
    [MaxLength(200, ErrorMessage = "Mô tả không được vượt quá 200 ký tự")]
    [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Mô tả không được chứa ký tự đặc biệt")]

    public string? Description { get; set; }
    [Range(1, double.MaxValue, ErrorMessage = "Số tiền không hợp lệ")]
    public decimal? Price { get; set; }
}
