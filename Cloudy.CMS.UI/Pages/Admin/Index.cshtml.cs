using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cloudy.CMS.UI.Pages.Admin
{
    [Authorize("adminarea")]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
