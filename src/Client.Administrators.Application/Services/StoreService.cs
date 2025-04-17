namespace ShopeeFoodClone.WebMvc.Administrators.Application.Services;

public class StoreService : IStoreService
{
    private readonly IBaseService _service;

    public StoreService(IBaseService service)
    {
        _service = service;
    }

    public async Task<Response?> GetStoresByVendorIdAsync(Guid vendorId)
    {
        return await _service.SendAsync(new Request()
        {
            ApiMethod = ApiMethod.Get,
            Url = $"{ApiUrlProperties.ApiGatewayUrl}/stores/vendor/{vendorId}",
        }, bearer: true);
    }
}
