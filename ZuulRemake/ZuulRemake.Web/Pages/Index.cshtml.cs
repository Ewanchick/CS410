using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ZuulRemake.Web.Pages
{
    public class IndexModel : PageModel
    {
        public IActionResult OnPostNewGame()
        {
            return RedirectToPage("PlayGame");
        }

        public IActionResult OnPostContinue()
        {
            // TO DO: load save data 
            return RedirectToPage("PlayGame");
        }

        public IActionResult OnPostQuit()
        {
            return RedirectToPage("Index");
        }
    }
}
