namespace ShopeeFoodClone.WebMvc.Administrators.Application.Models.Requests;

public sealed class VendorUpdateProductRequest : BaseProductRequest
{
    public Guid ConcurrencyStamp { get; set; }
}
