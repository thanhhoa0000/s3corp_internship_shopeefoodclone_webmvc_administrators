namespace ShopeeFoodClone.WebMvc.Administrators.Application;

public static partial class ServicesConfiguration
{
    public static IServiceCollection AddScopedServices(this IServiceCollection services)
    {
        services.AddScoped<IBaseService, BaseService>();
        services.AddScoped<ITokenProcessor, TokenProcessor>();
        services.AddScoped<IAccountService, AccountService>();
        
        return services;
    }
}
