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

    public async Task<Response?> GetProductByIdAsync(Guid productId)
    {
        return await _service.SendAsync(new Request()
        {
            ApiMethod = ApiMethod.Get,
            Url = $"{ApiUrlProperties.ApiGatewayUrl}/products/{productId}",
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

    public async Task<Response?> VendorUpdateProductAsync(VendorUpdateProductRequest request)
    {
        return await _service.SendAsync(new Request()
        {
            ApiMethod = ApiMethod.Put,
            Body = request,
            Url = $"{ApiUrlProperties.ApiGatewayUrl}/products/update-metadata",
        }, bearer: true);
    }

    public async Task<Response?> VendorDeleteProductAsync(Guid productId)
    {
        return await _service.SendAsync(new Request()
        {
            ApiMethod = ApiMethod.Delete,
            Url = $"{ApiUrlProperties.ApiGatewayUrl}/products/delete/{productId}",
        }, bearer: true);
    }
}
