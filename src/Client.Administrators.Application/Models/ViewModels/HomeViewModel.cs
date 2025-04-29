namespace ShopeeFoodClone.WebMvc.Administrators.Application.Models.ViewModels;

public sealed class HomeViewModel
{
    public List<StoreDto>? Stores { get; set; }
    public int PagesCount { get; set; } = 1;
    public int CurrentPage { get; set; } = 1;
    public int TotalStoresCount { get; set; } = 0;
}
