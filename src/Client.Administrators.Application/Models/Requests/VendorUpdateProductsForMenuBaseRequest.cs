namespace ShopeeFoodClone.WebMvc.Administrators.Application.Models.Requests;

public class VendorUpdateProductsForMenuBaseRequest
{
    public Guid MenuId { get; set; }
    public Guid ConcurrencyStamp { get; set; }
    public List<Guid>? ProductIds { get; set; }
}