namespace ShopeeFoodClone.WebMvc.Administrators.Presentation.Controllers;

public class ProductController : Controller
{
    private readonly IStoreService _storeService;
    private readonly IProductService _productService;
    private readonly ILogger<ProductController> _logger;
    private readonly string _imagesPath;

    public ProductController(
        IStoreService storeService,
        IProductService productService,
        ILogger<ProductController> logger,
        IConfiguration configuration)
    {
        _storeService = storeService;
        _productService = productService;       
        _logger = logger;
        _imagesPath = configuration.GetValue<string>("ImagesPath")!;
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
        try
        {
            var request = new CreateProductRequest
            {
                Name = model.Name!,
                Price = model.Price ?? Decimal.Zero,
                Description = model.Description!,
                StoreId = model.StoreId,
            };

            Response? createProductResponse = await _productService.CreateProductAsync(request);
            if (createProductResponse!.IsSuccessful)
            {
                var uploadFolder = Path.Combine(
                    _imagesPath,
                    "stores", model.StoreId.ToString(), 
                    "products", request.Id.ToString());

                if (!Directory.Exists(uploadFolder)) 
                    Directory.CreateDirectory(uploadFolder);

                var fileName = "cover-img.jpg";
                var customersFilePath = Path.Combine(uploadFolder, fileName);

                _logger.LogInformation($"Customers image path: {customersFilePath}");

                using var fileStream = new FileStream(customersFilePath, FileMode.Create);
                await model.CoverImage!.CopyToAsync(fileStream);

                TempData["success"] = "Tạo sản phẩm thành công";

                return RedirectToAction("Details", "Store", new { storeId = model.StoreId });
            }
            
            TempData["error"] = "Đã xảy ra lỗi";

            return View(model);
        }
        catch (Exception ex)
        {
            TempData["error"] = "Đã xảy ra lỗi";
            _logger.LogError($"Error occurred: {ex}");

            return View(model);
        }
    }
}