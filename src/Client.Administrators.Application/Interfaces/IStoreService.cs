namespace ShopeeFoodClone.WebMvc.Administrators.Application.Interfaces;

public interface IStoreService
{
    Task<Response?> GetStoresByVendorIdAsync(GetStoresByVendorIdRequest request);
    Task<Response?> GetStoreByIdAsync(Guid storeId);
    Task<Response?> CreateStoreAsync(CreateStoreRequest request);
    Task<Response?> VendorUpdateStoreAsync(VendorUpdateStoreRequest request);
    Task<Response?> GetStoresCount(GetStoresCountRequest request);
}
