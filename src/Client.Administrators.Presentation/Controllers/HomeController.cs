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
    
    [HttpGet]
    public IActionResult Index()
    {
        if (!User.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }
        
        return View(new HomeViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Index(Guid vendorId, int pageSize = 5, int pageNumber = 1)
    {
        var viewModel = new HomeViewModel();
        var stores = new List<StoreDto>();

        Response? storesResponse = await _storeService.GetStoresByVendorIdAsync(new GetStoresByVendorIdRequest
        {
            VendorId = vendorId,
            PageNumber = pageNumber,
            PageSize = pageSize,
        });
        
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
