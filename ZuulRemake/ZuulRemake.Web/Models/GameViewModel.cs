using ZuulRemake.Classes;

namespace ZuulRemake.Web.Models
{
    public class GameViewModel
    {
        public List<string> messages { get; set; } = new();
        public string currentRoomName { get; set; } = "";
        public string backgroundImageUrl { get; set; } = "";
        public string playerName { get; set; } = "";
        public int playerHP { get; set; }
        public int playerLevel { get; set; }
        public string RHandUrl { get; set; } = "";
        public string LHandUrl { get; set; } = "";
        public List<string> items { get; set; } = new();
        public List<string> monsters { get; set; } = new();
        public List<string> exits { get; set; } = new();
        public bool gameOver { get; set; }

        public static GameViewModel FromState(GameState state)
        {
            return new GameViewModel
            {
                currentRoomName = state.currentRoom?.Name ?? "",
                backgroundImageUrl = $"~/images/[state.currentRoom?.Name].png" ?? $"~/images/currentviewplaceholder/.png",
                playerName = state.player?.Name ?? "",
                playerHP = state.player?.HP ?? 0,
                playerLevel = state.player?.Level ?? 0,
                RHandUrl = "",
                LHandUrl = "",
                messages = new List<string>(state.messages) ?? new(),
                items = state.player?.Inventory?.Select(i => i.Name).ToList() ?? new(),
                monsters = state.currentRoom?.GetMonstersOb().Select(i => i.Name).ToList() ?? new(),
                exits = state.currentRoom?.GetExitsOb().Select(i => i.Direction).ToList() ?? new(),
                gameOver = state.player?.HP <= 0
            };
        }
    }
}
