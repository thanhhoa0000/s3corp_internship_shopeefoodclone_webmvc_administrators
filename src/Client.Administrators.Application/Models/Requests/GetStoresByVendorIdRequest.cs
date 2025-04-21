namespace ShopeeFoodClone.WebMvc.Administrators.Application.Models.Requests;

public class GetStoresByVendorIdRequest : BaseSearchRequest
{
    public Guid VendorId { get; set; }
    public bool IsPromoted { get; set; } = false;
}
