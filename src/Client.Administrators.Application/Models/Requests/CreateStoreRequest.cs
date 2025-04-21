namespace ShopeeFoodClone.WebMvc.Administrators.Application.Models.Requests;

public sealed record CreateStoreRequest(StoreDto Store, List<Guid> SubCateIds, string WardCode);
