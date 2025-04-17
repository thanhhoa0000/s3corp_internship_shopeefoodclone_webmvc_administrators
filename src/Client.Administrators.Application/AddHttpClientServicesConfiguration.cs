namespace ShopeeFoodClone.WebMvc.Administrators.Application;

public static partial class ServicesConfiguration
{
    public static IServiceCollection AddHttpClientServices(this IServiceCollection services)
    {
        services.AddHttpClient<ITokenProcessor, TokenProcessor>();
        services.AddHttpClient<IAccountService, AccountService>();
        services.AddHttpClient<IStoreService, StoreService>();
        
        return services;
    }
}
