namespace ShopeeFoodClone.WebMvc.Administrators.Application.Models.Requests;

public sealed class VendorUpdateMenuRequest : BaseMenuRequest
{
    public Guid ConcurrencyStamp { get; set; }
}
