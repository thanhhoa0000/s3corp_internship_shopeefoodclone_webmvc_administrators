namespace ShopeeFoodClone.WebMvc.Administrators.Application.Interfaces;

public interface IStoreService
{
    Task<Response?> GetStoresByVendorIdAsync(Guid vendorId);
}
