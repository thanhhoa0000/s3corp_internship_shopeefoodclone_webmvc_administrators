namespace ShopeeFoodClone.WebMvc.Administrators.Presentation.Controllers;

public class ProductController : Controller
{
    private readonly IStoreService _storeService;
    private readonly IProductService _productService;
    private readonly ILogger<ProductController> _logger;

    public ProductController(
        IStoreService storeService,
        IProductService productService,
        ILogger<ProductController> logger)
    {
        _storeService = storeService;
        _productService = productService;       
        _logger = logger;
    }
    
    [HttpGet]
    public IActionResult Create(Guid storeId) => View(new CreateProductViewModel { StoreId = storeId });
}