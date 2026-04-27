using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using ZuulRemake.Classes;
using static System.Net.Mime.MediaTypeNames;

namespace ZuulRemake.Web.Models
{
    public class GameState
    {
        public List<string> messages { get; set; } = new();
        public Player player { get; set; } = null!;
        public Room currentRoom => player.CurrentRoom;

        public List<Monster> monsters => currentRoom?.GetMonstersOb() ?? new();
        public List<Item> items => currentRoom?.GetItemsOb() ?? new();
        public List<Exit> exits => currentRoom?.GetExitsOb() ?? new();

        public GameState(Player p)
        {
            player = p;
        }

        public void EnterRoom(Room newRoom)
        {
            messages.Clear();
            messages.Add(newRoom.NarrativeDescription);
        }

        public void AddMessage(string message)
        {
            messages.Add(message);
        }

        public string PopMessage()
        {
            if (messages.Count == 0)
                return "";

            var msg = messages[0];
            messages.RemoveAt(0);
            return msg;
        }
    }
}
