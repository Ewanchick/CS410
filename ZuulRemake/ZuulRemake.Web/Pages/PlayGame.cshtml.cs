using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ZuulRemake.Web.Pages
{
    public class PlayGameModel : PageModel
    {
        private readonly ILogger<PlayGameModel> _logger;

        public PlayGameModel(ILogger<PlayGameModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}
