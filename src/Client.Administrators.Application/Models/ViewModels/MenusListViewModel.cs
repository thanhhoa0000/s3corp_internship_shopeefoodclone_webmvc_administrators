namespace ShopeeFoodClone.WebMvc.Administrators.Application.Models.ViewModels;

public sealed class MenusListViewModel
{
    public List<MenuDto>? Menus { get; set; }
    public Guid StoreId { get; set; }
}
