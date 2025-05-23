var clientName = "[Administrators] Web MVC";

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug($"Initializing {clientName}...\n-----\n");

try 
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    // Use NLog
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    
    builder.Services.AddDataProtection()
        .PersistKeysToFileSystem(new DirectoryInfo(@"/keys/"))
        .SetApplicationName("ShopeeFoodClone_Admin");
    
    builder.Services.AddControllersWithViews();
    builder.Services.AddHttpContextAccessor();
    
    builder.Services
        .AddHttpClient("ShopeeFoodClone_Admin")
        .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            });

    builder.Services.AddHttpClientServices();
    
    ApiUrlProperties.ApiGatewayUrl = builder.Configuration["GatewayUrl"];

    builder.Services.AddScopedServices();
    
    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/AccessDenied";
        });

    builder.Services.AddSession();
    
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSelfFrontend",
            policy =>
            {
                policy.AllowAnyOrigin()
                    .WithMethods("GET", "POST")
                    .AllowAnyHeader();
            });
    });

    var app = builder.Build();
    
    var imagesPath = builder.Configuration.GetValue<string>("ImagesPath");

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    
    app.UseStaticFiles(); 
    
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(imagesPath!),
        RequestPath = "/images"
    });


    app.UseRouting();
    
    app.UseCors("AllowSelfFrontend");
    
    app.UseAuthentication();

    app.UseAuthorization();
    
    app.UseMiddleware<TokenRefreshMiddleware>();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerFeature>();

            var exception = exceptionHandlerPathFeature?.Error;
            
            var errorMessage = exception?.ToString();
            logger.Error($"--------\nUnhandled exception:\n {errorMessage}\n--------");

            var error = new
            {
                error = "An unexpected error occurred."
            };

            var json = JsonSerializer.Serialize(error);

            await context.Response.WriteAsync(json);
        });
    });
    
    app.UseSession();

    app.Run();
}
catch (Exception ex)
{
    logger.Error($"Error(s) occured when starting {clientName}:\n-----\n{ex}");
}
finally
{
    LogManager.Shutdown();
}
