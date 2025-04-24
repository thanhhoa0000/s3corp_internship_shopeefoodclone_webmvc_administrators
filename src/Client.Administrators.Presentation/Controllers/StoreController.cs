namespace ShopeeFoodClone.WebMvc.Administrators.Presentation.Controllers;

public class StoreController : Controller
{
    private readonly IStoreService _storeService;
    private readonly IProductService _productService;
    private readonly ILogger<StoreController> _logger;
    private readonly string _imagesPath;

    public StoreController(
        IStoreService storeService,
        IProductService productService,
        ILogger<StoreController> logger,
        IConfiguration configuration)
    {
        _storeService = storeService;
        _productService = productService;
        _logger = logger;
        _imagesPath = configuration.GetValue<string>("ImagesPath")!;
    }
    
    [HttpGet]
    public async Task<IActionResult> Details(Guid storeId)
    {
        if (!User.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }
        
        var store = new StoreDto();
        var products = new List<ProductDto>();
        var menus = new List<MenuDto>();
        var viewModel = new StoreDetailsViewModel();
        
        Response? storeResponse = await _storeService.GetStoreByIdAsync(storeId);
        
        if (storeResponse!.IsSuccessful)
            store = JsonSerializer.Deserialize<StoreDto>(
                Convert.ToString(storeResponse.Body)!,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        Response? productsResponse = await _productService.GetProductsAsync(new GetProductsRequest
        {
            StoreId = storeId
        });
        
        if (productsResponse!.IsSuccessful)
            products = JsonSerializer.Deserialize<List<ProductDto>>(
                Convert.ToString(productsResponse.Body)!,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        
        Response? menusResponse = await _productService.GetMenusAsync(new GetMenusRequest
        {
            StoreId = storeId
        });
        
        if (menusResponse!.IsSuccessful)
            menus = JsonSerializer.Deserialize<List<MenuDto>>(
                Convert.ToString(menusResponse.Body)!,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        
        viewModel.Store = store;
        viewModel.Products = products.Where(p => p.State != ProductState.Deleted).ToList();
        viewModel.Menus = menus;
        
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> VendorUpdate(Guid storeId)
    {
        if (!User.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }
        
        var viewModel = new UpdateStoreViewModel();
        var store = new StoreDto();
        
        Response? storeResponse = await _storeService.GetStoreByIdAsync(storeId);
        
        if (storeResponse!.IsSuccessful)
            store = JsonSerializer.Deserialize<StoreDto>(
                Convert.ToString(storeResponse.Body)!,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        
        viewModel.StoreId = store.Id;
        viewModel.Name = store.Name;
        viewModel.CoverImagePath = store.CoverImagePath;
        viewModel.ConcurrencyStamp = store.ConcurrencyStamp;
        viewModel.OpeningHour = store.OpeningHour;
        viewModel.ClosingHour = store.ClosingHour;
        viewModel.StreetName = store.StreetName;
        viewModel.WardCode = store.WardCode;
        viewModel.DistrictCode = store.Ward!.DistrictCode;
        viewModel.ProvinceCode = store.Ward!.District!.ProvinceCode;
        
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> VendorUpdate(UpdateStoreViewModel model)
    {
        if (!User.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }

        var request = new VendorUpdateStoreRequest
        {
            Id = model.StoreId,
            Name = model.Name,
            StreetName = model.StreetName,
            WardCode = model.WardCode,
            CoverImagePath = model.CoverImagePath,
            ConcurrencyStamp = model.ConcurrencyStamp,
            OpeningHour = model.OpeningHour,
            ClosingHour = model.ClosingHour,
        };
        
        Response? vendorUpdateStoreResponse = await _storeService.
        
        return RedirectToAction("Details", "Store", new { storeId = model.StoreId });
    }
}
