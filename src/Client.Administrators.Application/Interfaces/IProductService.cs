namespace ShopeeFoodClone.WebMvc.Administrators.Application.Interfaces;

public interface IProductService
{
    Task<Response?> GetProductsAsync(GetProductsRequest request);
    Task<Response?> GetMenusAsync(GetMenusRequest request);
}
