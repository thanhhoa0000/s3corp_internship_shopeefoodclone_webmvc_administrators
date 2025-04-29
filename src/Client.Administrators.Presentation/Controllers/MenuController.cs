namespace ShopeeFoodClone.WebMvc.Administrators.Presentation.Controllers;

public class MenuController : Controller
{
    private readonly IMenuService _menuService;
    private readonly ILogger<MenuController> _logger;

    public MenuController(
        IMenuService menuService,
        ILogger<MenuController> logger)
    {
        _menuService = menuService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> List(Guid storeId)
    {
        if (!User.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }

        var viewModel = new MenusListViewModel();
        var menus = new List<MenuDto>();
        var request = new GetMenusRequest
        {
            StoreId = storeId
        };
        
        Response? getMenusResponse = await _menuService.GetMenusByStoreIdAsync(request);
        
        if (getMenusResponse!.IsSuccessful)
            menus = JsonSerializer.Deserialize<List<MenuDto>>(
                Convert.ToString(getMenusResponse.Body)!,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        
        viewModel.Menus = menus.Where(m => m.State != MenuState.Inactive).ToList();
        
        return View(viewModel);
    }
    
    [HttpGet]
    public IActionResult VendorCreate(Guid storeId)
    {
        if (!User.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }

        return View(new CreateMenuViewModel() { StoreId = storeId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> VendorCreate(CreateMenuViewModel model)
    {
        var request = new CreateMenuRequest
        {
            StoreId = model.StoreId,
            Title = model.Title ?? "",
        };
        
        Response? createMenuResponse = await _menuService.VendorAddMenuAsync(request);

        if (createMenuResponse!.IsSuccessful)
            TempData["success"] = "Tạo menu thành công";
        else
            TempData["error"] = "Đã xảy ra lỗi";
        
        return RedirectToAction("Details", "Store", new { storeId = model.StoreId });
    }

    [HttpPost]
    public async Task<IActionResult> VendorUpdate([FromBody] VendorUpdateMenuRequest request)
    {
        Response? vendorUpdateMenuResponse = await _menuService.VendorUpdateMenuAsync(request);
        
        if (vendorUpdateMenuResponse!.IsSuccessful)
            return Json(new { success = true, message = "Cập nhật menu thành công" });
        
        return Json(new { success = false, message = "Đã xảy ra lỗi" });
    }
    
    [HttpPost]
    public async Task<IActionResult> VendorDelete([FromBody] Guid menuId)
    {
        Response? vendorDeleteMenuResponse = await _menuService.VendorDeleteMenuAsync(menuId);
        
        _logger.LogDebug($"Menu id: {menuId}");
        
        if (vendorDeleteMenuResponse!.IsSuccessful)
            return Json(new { success = true, message = "Cập nhật menu thành công" });
        
        return Json(new { success = false, message = "Đã xảy ra lỗi" });
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

    [HttpPost]
    public async Task<IActionResult> RemoveProductsFromMenu(Guid menuId, string[] productIds)
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
    
        var request = new VendorRemoveProductsFromMenuRequest
        {
            MenuId = menuId,
            ConcurrencyStamp = menu.ConcurrencyStamp,
            ProductIds = productIds.Select(s => Guid.Parse(s)).ToList()
        };
    
        Response? addProductsToMenuResponse = await _menuService.VendorRemoveProductsFromMenuAsync(request);
    
        if (addProductsToMenuResponse!.IsSuccessful)
        {
            return Json(new { success = true, message = "Cập nhật menu thành công" });
        }
    
        return Json(new { success = false, message = "Đã xảy ra lỗi" });
    }
}
