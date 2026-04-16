using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZuulRemake.Classes;
using ZuulRemake.Web.Models;

namespace ZuulRemake.Web.Pages
{
    public class PlayGameModel : PageModel
    {
        public GameState GameState { get; set; }

        // init here or constructor?
        public void OnGet()
        {
            var game = new Game(new Player("Test Player"));
            var player = game.Player;
            GameState = new(player);
        }
    }
}
