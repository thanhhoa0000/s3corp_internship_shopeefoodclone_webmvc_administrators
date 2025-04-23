namespace ShopeeFoodClone.WebMvc.Administrators.Presentation.Controllers;

public class ProductController : Controller
{
    private readonly IStoreService _storeService;
    private readonly IProductService _productService;
    private readonly ILogger<ProductController> _logger;
    private readonly string _customersImagePath;
    private readonly string _administratorsImagePath;

    public ProductController(
        IStoreService storeService,
        IProductService productService,
        ILogger<ProductController> logger,
        IConfiguration configuration)
    {
        _storeService = storeService;
        _productService = productService;       
        _logger = logger;
        _customersImagePath = configuration.GetValue<string>("ImagePathCustomers")!;
        _administratorsImagePath = configuration.GetValue<string>("ImagePathAdministrators")!;
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
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("ModelState invalid: " + string.Join("; ",
                        ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                    return View(model);
                }

                if (model.CoverImage == null || model.CoverImage.Length == 0)
                {
                    TempData["error"] = "Bạn chưa chọn hình ảnh hợp lệ";
                    return View(model);
                }

                var customersUploadsFolder = Path.Combine(_customersImagePath, model.StoreId.ToString(), "products",
                    request.Id.ToString());
                var administratorsUploadsFolder = Path.Combine(_administratorsImagePath, model.StoreId.ToString(),
                    "products", request.Id.ToString());

                if (!Directory.Exists(customersUploadsFolder)) 
                    Directory.CreateDirectory(customersUploadsFolder);
                
                if (!Directory.Exists(administratorsUploadsFolder))
                    Directory.CreateDirectory(administratorsUploadsFolder);

                var fileName = "cover-img.jpg";
                var customersFilePath = Path.Combine(customersUploadsFolder, fileName);
                var administratorsFilePath = Path.Combine(administratorsUploadsFolder, fileName);

                _logger.LogInformation($"Customers image path: {customersFilePath}");

                using var fileStream = new FileStream(customersFilePath, FileMode.Create);
                await model.CoverImage!.CopyToAsync(fileStream);

                System.IO.File.Copy(customersFilePath, administratorsFilePath, overwrite: true);

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