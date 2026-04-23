using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZuulRemake.Classes;
using ZuulRemake.Services;
using ZuulRemake.Web.Models;

namespace ZuulRemake.Web.Pages
{
    public class PlayGameModel : PageModel
    {
        private GameState GameState;
        private readonly WebGameService _gameService;

        public PlayGameModel()
        {
            var game = new Game(new Player("Test Player"));
            var player = game.Player;
            GameState = new(player);
            _gameService = new WebGameService();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            
            await _gameService.
            return Page();
        }
    }
}
