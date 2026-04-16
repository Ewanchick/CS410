using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZuulRemake.Classes;
using ZuulRemake.Web.Models;

namespace ZuulRemake.Web.Pages
{
    public class PlayGameModel : PageModel
    {
        private readonly ILogger<PlayGameModel> _logger;
        public GameState GameState { get; set; } = new();

        public PlayGameModel(ILogger<PlayGameModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            var game = new Game(new Player("Test Player"));
            var player = game.Player;

            string roomName = "ERROR! ";
            string roomDesc = "Room not found. ";

            try
            {
                var room = player.GetCurrentRoom();

                roomName = room.Name;
                roomDesc = room.GetLongDescription();
            }
            catch (Exception ex)
            {
                roomDesc = $"Error: {ex.Message}";
            }

            GameState = new GameState
            {
                RoomName = roomName,
                RoomDescription = roomDesc,
                PlayerHP = player.HP,
                Inventory = player.GetItems().Select(i => i.Name).ToList(),
                Messages = new List<string> { "Game loaded." }
            };
        }
    }
}
