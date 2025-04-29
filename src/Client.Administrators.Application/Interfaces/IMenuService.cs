namespace ShopeeFoodClone.WebMvc.Administrators.Application.Interfaces;

public interface IMenuService
{
    Task<Response?> VendorAddMenuAsync(CreateMenuRequest request);
    Task<Response?> GetMenuByIdAsync(Guid menuId);
    Task<Response?> GetMenusByStoreIdAsync(GetMenusRequest request);
    Task<Response?> VendorUpdateMenuAsync(VendorUpdateMenuRequest request);
    Task<Response?> VendorAddProductsToMenuAsync(VendorAddProductsToMenuRequest request);
    Task<Response?> VendorRemoveProductsFromMenuAsync(VendorRemoveProductsFromMenuRequest request);
    Task<Response?> VendorDeleteMenuAsync(Guid menuId);
}