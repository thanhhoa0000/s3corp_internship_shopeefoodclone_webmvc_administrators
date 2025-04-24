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
                var filePath = Path.Combine(uploadFolder, fileName);

                _logger.LogInformation($"Customers image path: {filePath}");

                using var fileStream = new FileStream(filePath, FileMode.Create);
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

    [HttpPost]
    public async Task<IActionResult> VendorDelete([FromForm] Guid productId)
    {
        if (!User.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }

        var product = new ProductDto();

        Response? productResponse = await _productService.GetProductByIdAsync(productId);
        if (productResponse!.IsSuccessful)
            product = JsonSerializer.Deserialize<ProductDto>(
                Convert.ToString(productResponse.Body)!,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        var storeId = product.StoreId;

        Response? vendorDeleteResponse = await _productService.VendorDeleteProductAsync(productId);

        if (vendorDeleteResponse!.IsSuccessful)
            TempData["success"] = "Xoá sản phẩm thành công";
        else
            TempData["error"] = "Đã xảy ra lỗi";

        return RedirectToAction("Details", "Store", new { storeId });
    }

    [HttpGet]
    public async Task<IActionResult> VendorUpdate(Guid productId)
    {
        if (!User.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }

        var product = new ProductDto();
        var viewModel = new UpdateProductViewModel();

        Response? productResponse = await _productService.GetProductByIdAsync(productId);

        if (productResponse!.IsSuccessful)
            product = JsonSerializer.Deserialize<ProductDto>(
                Convert.ToString(productResponse.Body)!,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        viewModel.ProductId = product.Id;
        viewModel.StoreId = product.StoreId;
        viewModel.Name = product.Name;
        viewModel.Price = product.Price;
        viewModel.Discount = product.Discount;
        viewModel.Description = product.Description;
        viewModel.CoverImagePath = product.CoverImagePath;
        viewModel.ConcurrencyStamp = product.ConcurrencyStamp;
        viewModel.AvailableStock = product.AvailableStock;

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> VendorUpdate(UpdateProductViewModel model)
    {
        if (!User.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }

        var request = new VendorUpdateProductRequest
        {
            Id = model.ProductId,
            StoreId = model.StoreId,
            Name = model.Name!,
            Price = model.Price ?? Decimal.Zero,
            Discount = model.Discount ?? Decimal.Zero,
            Description = model.Description!,
            ConcurrencyStamp = model.ConcurrencyStamp ?? Guid.Empty,
            CoverImagePath = model.CoverImagePath,
            AvailableStock = model.AvailableStock ?? 0,
        };

        Response? vendorUpdateResponse = await _productService.VendorUpdateProductAsync(request);

        if (vendorUpdateResponse!.IsSuccessful)
        {
            if (model.CoverImage is not null)
            {
                var filePath = Path.Combine(_imagesPath,
                    model.CoverImagePath!.TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));

                _logger.LogInformation($"Customers image path: {filePath}");

                using var fileStream = new FileStream(filePath, FileMode.Create);
                await model.CoverImage!.CopyToAsync(fileStream);
            }

            TempData["success"] = "Cập nhật thông tin sản phẩm thành công";

            return RedirectToAction("Details", "Store", new { storeId = model.StoreId });
        }

        TempData["error"] = "Đã xảy ra lỗi";

        return View(model);
    }
}