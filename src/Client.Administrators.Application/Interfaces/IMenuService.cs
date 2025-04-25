namespace ShopeeFoodClone.WebMvc.Administrators.Application.Interfaces;

public interface IMenuService
{
    Task<Response?> VendorAddMenuAsync(CreateMenuRequest request);
    Task<Response?> GetMenusByStoreIdAsync(GetMenusRequest request);
    Task<Response?> VendorUpdateMenuAsync(VendorUpdateMenuRequest request);
    Task<Response?> VendorAddProductsToMenuAsync(AddProductsToMenuRequest request);
}