namespace ShopeeFoodClone.WebMvc.Administrators.Presentation.Controllers;

public class StoreController : Controller
{
    private readonly IStoreService _storeService;
    private readonly IProductService _productService;
    private readonly IMenuService _menuService;
    private readonly ILogger<StoreController> _logger;
    private readonly string _imagesPath;

    public StoreController(
        IStoreService storeService,
        IProductService productService,
        IMenuService menuService,
        ILogger<StoreController> logger,
        IConfiguration configuration)
    {
        _storeService = storeService;
        _productService = productService;
        _menuService = menuService;
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
        
        Response? menusResponse = await _menuService.GetMenusByStoreIdAsync(new GetMenusRequest
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
    [ValidateAntiForgeryToken]
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

        Response? vendorUpdateStoreResponse = await _storeService.VendorUpdateStoreAsync(request);

        if (vendorUpdateStoreResponse!.IsSuccessful)
        {
            if (model.CoverImage is not null)
            {
                var filePath = Path.Combine(_imagesPath,
                    model.CoverImagePath!.TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));

                _logger.LogInformation($"Image path: {filePath}");

                using var fileStream = new FileStream(filePath, FileMode.Create);
                await model.CoverImage!.CopyToAsync(fileStream);
            }

            TempData["success"] = "Cập nhật thông tin cửa hàng thành công";

            return RedirectToAction("Details", "Store", new { storeId = model.StoreId });
        }

        TempData["error"] = "Đã xảy ra lỗi";
        
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddProductsToMenu(Guid menuId, string[] productIds)
    {
        if (!User.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }
        
        var menu = new MenuDto();
        var store = new StoreDto();
        var menus = new List<MenuDto>();
        var products = new List<ProductDto>();
        
        Response? menuResponse = await _menuService.GetMenuByIdAsync(menuId);
        
        if (menuResponse!.IsSuccessful)
            menu = JsonSerializer.Deserialize<MenuDto>(
                Convert.ToString(menuResponse.Body)!,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    
        var request = new VendorAddProductsToMenuRequest
        {
            MenuId = menuId,
            ConcurrencyStamp = menu.ConcurrencyStamp,
            ProductIds = productIds.Select(s => Guid.Parse(s)).ToList()
        };
    
        Response? addProductsToMenuResponse = await _menuService.VendorAddProductsToMenuAsync(request);
    
        if (addProductsToMenuResponse!.IsSuccessful)
        {
            return Json(new { success = true, message = "Cập nhật menu thành công" });
        }
    
        return Json(new { success = false, message = "Đã xảy ra lỗi" });
    }
}
