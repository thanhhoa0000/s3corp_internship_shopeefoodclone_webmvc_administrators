namespace ShopeeFoodClone.WebMvc.Administrators.Presentation.Controllers;

public class ProductController : Controller
{
    private readonly IStoreService _storeService;
    private readonly IProductService _productService;
    private readonly ILogger<ProductController> _logger;
    private readonly string _imagePath;

    public ProductController(
        IStoreService storeService,
        IProductService productService,
        ILogger<ProductController> logger,
        IConfiguration configuration)
    {
        _storeService = storeService;
        _productService = productService;       
        _logger = logger;
        _imagePath = configuration.GetValue<string>("ImagePath")!;
    }
    
    [HttpGet]
    public IActionResult Create(Guid storeId)
    {
        if (!User.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }
        
        return View(new CreateProductViewModel { StoreId = storeId });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductViewModel model)
    {
        var request = new CreateProductRequest
        {
            Name = model.Name!,
            Price = model.Price ?? Decimal.Zero,
            Description = model.Description!,
            StoreId = model.StoreId,
        };
        
        request.Id = Guid.NewGuid();
        
        Response? createProductResponse = await _productService.CreateProductAsync(request);
        if (!createProductResponse!.IsSuccessful)
        {
            var uploadsFolder = Path.Combine(_imagePath, model.StoreId.ToString(), "products", request.Id.ToString());
            
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = "cover.jpg";
            var filePath = Path.Combine(uploadsFolder, fileName);
            
            using var stream = new FileStream(filePath, FileMode.Create);
            await model.CoverImage!.CopyToAsync(stream);
            
            TempData["success"] = "Tạo sản phẩm thành công";
            
            return RedirectToAction("Details", "Store", new { storeId = model.StoreId });
        }

        TempData["error"] = "Đã xảy ra lỗi";
        
        return View(model);
    }
}