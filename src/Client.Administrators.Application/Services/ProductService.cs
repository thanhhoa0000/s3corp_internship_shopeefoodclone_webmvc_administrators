namespace ShopeeFoodClone.WebMvc.Administrators.Application.Services;

public class ProductService : IProductService
{
    private readonly IBaseService _service;

    public ProductService(IBaseService service)
    {
        _service = service;
    }

    public async Task<Response?> GetProductsAsync(GetProductsRequest request)
    {
        return await _service.SendAsync(new Request()
        {
            ApiMethod = ApiMethod.Post,
            Body = request,
            Url = $"{ApiUrlProperties.ApiGatewayUrl}/products/get-from-store",
        }, bearer: true);
    }

    public async Task<Response?> GetMenusAsync(GetMenusRequest request)
    {
        return await _service.SendAsync(new Request()
        {
            ApiMethod = ApiMethod.Post,
            Body = request,
            Url = $"{ApiUrlProperties.ApiGatewayUrl}/menus/get-from-store",
        }, bearer: true);
    }

    public async Task<Response?> CreateProductAsync(CreateProductRequest request)
    {
        return await _service.SendAsync(new Request()
        {
            ApiMethod = ApiMethod.Post,
            Body = request,
            Url = $"{ApiUrlProperties.ApiGatewayUrl}/products",
        }, bearer: true);
    }
}
