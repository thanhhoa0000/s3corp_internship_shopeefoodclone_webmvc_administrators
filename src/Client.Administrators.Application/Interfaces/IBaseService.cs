namespace ShopeeFoodClone.WebMvc.Administrators.Application.Interfaces;

public interface IBaseService
{
    Task<Response?> SendAsync(Request request, bool bearer = true);
}