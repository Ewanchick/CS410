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
        public Room currentRoom { get; set; } = null!;

        public List<Monster> monsters => currentRoom.GetMonstersOb();
        public List<Item> items => currentRoom.GetItemsOb();
        public List<Exit> exits => currentRoom.GetExitsOb();

        public GameState(Player p)
        {
            player = p;
            currentRoom = player.CurrentRoom;
        }

        public GameState()
        {

        }
    }
}
