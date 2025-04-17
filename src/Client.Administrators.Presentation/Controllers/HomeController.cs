namespace ShopeeFoodClone.WebMvc.Administrators.Presentation.Controllers;

public class HomeController : Controller
{
    private readonly IStoreService _storeService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(
        IStoreService storeService,
        ILogger<HomeController> logger)
    {
        _storeService = storeService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        if (!User.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }
        
        var viewModel = new HomeViewModel();
        var vendorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var stores = new List<StoreDto>();
        
        Response? storesResponse = await _storeService.GetStoresByVendorIdAsync(Guid.Parse(vendorId!));
        
        if (storesResponse!.IsSuccessful)
            stores = JsonSerializer.Deserialize<List<StoreDto>>(
                Convert.ToString(storesResponse.Body)!,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        
        viewModel.Stores = stores;
        
        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
