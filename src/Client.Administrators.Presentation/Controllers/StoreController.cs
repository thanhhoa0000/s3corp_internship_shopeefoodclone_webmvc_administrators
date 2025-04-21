namespace ShopeeFoodClone.WebMvc.Administrators.Presentation.Controllers;

public class StoreController : Controller
{
    private readonly IStoreService _storeService;
    private readonly IProductService _productService;
    private readonly ILogger<StoreController> _logger;

    public StoreController(
        IStoreService storeService,
        IProductService productService,
        ILogger<StoreController> logger)
    {
        _storeService = storeService;
        _productService = productService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Create()
    {
        if (!User.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }

        return View(new CreateStoreViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateStoreViewModel model)
    {
        var request = new CreateStoreRequest(
            new StoreDto
            {
                UserId = model.VendorId,
                Name = model.Name,
                StreetName = model.StreetName,
                OpeningHour = model.OpeningHour,
                ClosingHour = model.ClosingHour
            },
            model.SubCategoryIds,
            model.WardCode!);

        Response? createStoreResponse = await _storeService.CreateStoreAsync(request);

        if (!createStoreResponse!.IsSuccessful)

            TempData["error"] = "Đã xảy ra lỗi khi tạo cửa hàng!";

        else
            TempData["success"] = "Tạo cửa hàng thành công!";

        return RedirectToAction("Index", "Home");
    }
    
    [HttpGet]
    public async Task<IActionResult> Details(Guid storeId)
    {
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
        viewModel.Products = products;
        viewModel.Menus = menus;
        
        return View(viewModel);
    }
}
