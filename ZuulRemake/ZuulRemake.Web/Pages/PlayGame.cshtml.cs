using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZuulRemake.Classes;
using ZuulRemake.Web.Models;
using ZuulRemake.Web.Helpers;

namespace ZuulRemake.Web.Pages
{
    public class PlayGameModel : PageModel
    {
        public GameViewModel ViewModel { get; set; } = new();

        public IGameService _gameService;

        public PlayGameModel(IGameService gameService)
        {
            _gameService = gameService;
        }

        public IActionResult OnGet()
        {
            var state = _gameService.CreateNewGame();
            var save = _gameService.ToSaveDto(state);
            HttpContext.Session.SetJson("GameSave", save);

            ViewModel = GameViewModel.FromState(state);
            return Page();
        }

        public IActionResult OnPostMove(string direction)
        {
            var save = HttpContext.Session.GetJson<GameSaveDto>("GameSave");
            if (save == null)
                return RedirectToPage();

            var state = _gameService.LoadFromSave(save);
            state = _gameService.Move(state, direction);

            var updatedSave = _gameService.ToSaveDto(state);
            HttpContext.Session.SetJson("GameSave", updatedSave);

            ViewModel = GameViewModel.FromState(state);
            return Page();
        }

        public IActionResult OnPostAttack(string direction)
        {
            return Page();
        }

        public IActionResult OnPostTakeItem(string direction)
        {
            return Page();
        }
    }
}