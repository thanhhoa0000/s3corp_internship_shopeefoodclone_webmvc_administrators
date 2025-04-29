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
    public async Task<IActionResult> Index(
        Guid vendorId,
        int pageSize = 5, 
        int pageNumber = 1)
    {
        var viewModel = new HomeViewModel();
        var stores = new List<StoreDto>();
        var storesCount = 0;
        
        Response? storesCountResponse = await _storeService.GetStoresCount(new GetStoresCountRequest
        {
            VendorId = vendorId
        });
        
        _logger.LogDebug($"vendorId: {vendorId}");

        if (storesCountResponse!.IsSuccessful)
            storesCount = JsonSerializer.Deserialize<int>(
                Convert.ToString(storesCountResponse.Body)!,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

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
        
        viewModel.Stores = stores.Where(s => s.State != StoreState.Deleted).ToList();
        viewModel.PagesCount = (int)Math.Ceiling((double)storesCount / pageSize);
        viewModel.CurrentPage = pageNumber;
        viewModel.TotalStoresCount = storesCount;
        
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
