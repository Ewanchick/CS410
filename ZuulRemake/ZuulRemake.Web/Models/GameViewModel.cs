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
        public string roomItemUrl { get; set; } = "";
        public List<string> inventory { get; set; } = new();
        public List<string> items { get; set; } = new();
        public List<string> monsters { get; set; } = new();
        public List<string> exits { get; set; } = new();
        public bool gameOver { get; set; }
        public bool isLit { get; set; }


        public static GameViewModel FromState(GameState state)
        {
            // START: why is background image not working
            var roomName = state.currentRoom?.Name.ToLower() ?? "entryway";

            string backgroundUrl = state.roomLit ? $"~/images/{roomName}.png" 
                : $"~/images/{roomName}_dark.png";
            // END

            List<string> roomItems = (!state.roomLit) ? new List<string>() 
                : (state.currentRoom?.GetItemsOb()?.Select(i => i.Name).ToList() ?? new());

            return new GameViewModel
            {
                currentRoomName = state.currentRoom?.Name ?? "",


                backgroundImageUrl = state.currentRoom?.Name ?? "",
                playerName = state.player?.Name ?? "",
                playerHP = state.player?.HP ?? 0,
                playerLevel = state.player?.Level ?? 0,

                RHandUrl = state.swordHeld ? "~/images/rhand_sword.png" : "~/images/rhand.png",
                LHandUrl = "~/images/lhand.png",

                messages = state.messages != null ? new List<string>(state.messages) : new List<string>(),

                items = state.currentRoom?.GetItemsOb()?.Select(i => i.Name).ToList() ?? new(),
                roomItemUrl = state.currentRoom != null ? $"~/images/{state.items?.FirstOrDefault()?.Name.Replace(" ", "").ToLower()}.png" : "",

                inventory = state.player?.Inventory?.Select(i => i.Name).ToList() ?? new(),
                monsters = state.currentRoom?.GetMonstersOb().Select(i => i.Name).ToList() ?? new(),
                exits = state.currentRoom?.GetExitsOb().Select(i => i.Direction).ToList() ?? new(),

                isLit = state.roomLit,
                gameOver = state.player?.HP <= 0
            };
        }
    }
}
