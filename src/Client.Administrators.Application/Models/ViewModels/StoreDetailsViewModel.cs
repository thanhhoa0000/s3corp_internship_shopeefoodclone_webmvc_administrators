namespace ShopeeFoodClone.WebMvc.Administrators.Application.Models.ViewModels;

public sealed class StoreDetailsViewModel
{
    public StoreDto? Store { get; set; }
    public List<MenuDto>? Menus { get; set; }
    public List<ProductDto>? Products { get; set; }
}
