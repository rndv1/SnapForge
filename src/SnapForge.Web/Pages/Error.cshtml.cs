using Microsoft.AspNetCore.Mvc.RazorPages;
using SnapForge.Web.Localization;

namespace SnapForge.Web.Pages;

public sealed class ErrorModel : PageModel
{
    public WebCopy Text { get; private set; } = WebCopy.For(WebCopy.RussianCode);

    public void OnGet()
    {
        Text = WebCopy.For(Request.Query["lang"].ToString());
    }
}
