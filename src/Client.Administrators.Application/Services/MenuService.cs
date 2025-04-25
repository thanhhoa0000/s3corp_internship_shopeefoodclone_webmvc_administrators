namespace ShopeeFoodClone.WebMvc.Administrators.Application.Services;

public class MenuService : IMenuService
{
    private readonly IBaseService _service;

    public MenuService(IBaseService service)
    {
        _service = service;
    }
    
    public async Task<Response?> GetMenusByStoreIdAsync(GetMenusRequest request)
    {
        return await _service.SendAsync(new Request()
        {
            ApiMethod = ApiMethod.Post,
            Body = request,
            Url = $"{ApiUrlProperties.ApiGatewayUrl}/menus/get-from-store",
        }, bearer: false);
    }

    public async Task<Response?> VendorAddMenuAsync(CreateMenuRequest request)
    {
        return await _service.SendAsync(new Request()
        {
            ApiMethod = ApiMethod.Post,
            Body = request,
            Url = $"{ApiUrlProperties.ApiGatewayUrl}/menus",
        }, bearer: true);
    }

    public async Task<Response?> VendorUpdateMenuAsync(VendorUpdateMenuRequest request)
    {
        return await _service.SendAsync(new Request()
        {   
            ApiMethod = ApiMethod.Put,
            Body = request,
            Url = $"{ApiUrlProperties.ApiGatewayUrl}/menus/update-metadata"
        }, bearer: true);
    }

    public async Task<Response?> VendorAddProductsToMenuAsync(AddProductsToMenuRequest request)
    {
        return await _service.SendAsync(new Request()
        {
            ApiMethod = ApiMethod.Post,
            Body = request,
            Url = $"{ApiUrlProperties.ApiGatewayUrl}/menus/add-products",
        });
    }
}