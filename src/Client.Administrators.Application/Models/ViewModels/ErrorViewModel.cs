namespace ShopeeFoodClone.WebMvc.Administrators.Application.Models.ViewModels;

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
