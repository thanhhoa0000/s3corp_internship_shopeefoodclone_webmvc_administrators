namespace ShopeeFoodClone.WebMvc.Administrators.Application.Interfaces;

public interface IProductService
{
    Task<Response?> GetProductsAsync(GetProductsRequest request);
    Task<Response?> GetProductByIdAsync(Guid productId);
    Task<Response?> GetMenusAsync(GetMenusRequest request);
    Task<Response?> CreateProductAsync(CreateProductRequest request);
    Task<Response?> VendorDeleteProductAsync(Guid productId);
    Task<Response?> VendorUpdateProductAsync(VendorUpdateProductRequest request);
}
