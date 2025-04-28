namespace ShopeeFoodClone.WebMvc.Administrators.Application.Services;

public class StoreService : IStoreService
{
    private readonly IBaseService _service;

    public StoreService(IBaseService service)
    {
        _service = service;
    }

    public async Task<Response?> GetStoresByVendorIdAsync(GetStoresByVendorIdRequest request)
    {
        return await _service.SendAsync(new Request()
        {
            ApiMethod = ApiMethod.Post,
            Body = request,
            Url = $"{ApiUrlProperties.ApiGatewayUrl}/stores/vendor",
        }, bearer: true);
    }
    
    public async Task<Response?> GetStoreByIdAsync(Guid storeId)
    {
        return await _service.SendAsync(new Request()
        {
            ApiMethod = ApiMethod.Get,
            Url = $"{ApiUrlProperties.ApiGatewayUrl}/stores/{storeId}",
        }, bearer: true);
    }

    public async Task<Response?> CreateStoreAsync(CreateStoreRequest request)
    {
        return await _service.SendAsync(new Request()
        {
            ApiMethod = ApiMethod.Post,
            Body = request,
            Url = $"{ApiUrlProperties.ApiGatewayUrl}/stores",
        }, bearer: true);
    }

    public async Task<Response?> VendorUpdateStoreAsync(VendorUpdateStoreRequest request)
    {
        return await _service.SendAsync(new Request()
        {
            ApiMethod = ApiMethod.Put,
            Body = request,
            Url = $"{ApiUrlProperties.ApiGatewayUrl}/stores/update-by-vendor",
        }, bearer: true);
    }
    
    public async Task<Response?> GetStoresCount(GetStoresCountRequest request)
    {
        return await _service.SendAsync(new Request()
        {
            ApiMethod = ApiMethod.Post,
            Body = request,
            Url = $"{ApiUrlProperties.ApiGatewayUrl}/stores/get-count"
        }, bearer: false);
    }
}
