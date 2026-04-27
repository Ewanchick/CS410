using System.Runtime.CompilerServices;
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
        public List<Item> inventory => player.Inventory ?? new();

        public List<Monster> monsters => currentRoom?.GetMonstersOb() ?? new();
        public List<Item> items => currentRoom?.GetItemsOb() ?? new();
        public List<Exit> exits => currentRoom?.GetExitsOb() ?? new();

        public GameState(Player p)
        {
            player = p;
            p.AddItem(new Item("sword", "", 1, 0));
            p.AddItem(new Item("dffd", "", 1, 0));
            p.AddItem(new Item("dfdf", "", 1, 0));
            p.AddItem(new Item("swofdrd", "", 1, 0));
            p.AddItem(new Item("lantfgern", "", 1, 0));
            currentRoom.AddItem(new Item("sword", "", 1, 0));
        }

        public Item GetInventoryItem(string itemName)
        {
            var item = inventory.FirstOrDefault(i => i.Name != null && i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
            if (item != null) return item;
            else
            {
                throw new InvalidOperationException($"Item with name {itemName} not found.");
            }
        }

        public Item GetRoomItem(string itemName)
        {
            var item = items.FirstOrDefault(i => i.Name != null && i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
            if (item != null) return item;
            else
            {
                throw new InvalidOperationException($"Item with name {itemName} not found.");
            }
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
