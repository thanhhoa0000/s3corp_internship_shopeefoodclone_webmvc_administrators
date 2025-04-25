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
}