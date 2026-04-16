using System.Security.Cryptography.X509Certificates;
using ZuulRemake.Classes;
using static System.Net.Mime.MediaTypeNames;

namespace ZuulRemake.Web.Models
{
    public class GameState
    {
        public List<string> messages = new();

        public Player player;
        public string LHand = "~/images/lhand.png";
        public string RHand = "~/images/rhand.png";

        public Room currentRoom;
        public string roomUrl = "~/images/currentviewplaceholder2.png"; // fallback

        public List<Monster>? monsters;
        public List<Item>? items;
        public List<Exit>? exits;

        public List<string> monsterUrls = new();
        public List<string> itemUrls = new();
        public List<string> exitNames = new();

        /* Access some properties directly for display purposes (Monster name, Exit direction, ?)
         * We must be able to update properties whenever GameState changes - need methods for that
         * When are they going to be called? - every time game state changes (button pressed)
         * 
         */

        public GameState(Player p)
        {
            player = p;
            currentRoom = player.CurrentRoom;
            monsters = currentRoom.GetMonstersOb();
            items = currentRoom.GetItemsOb();
            exits = currentRoom.GetExitsOb();

            roomUrl = SetBackgroundState(currentRoom);
            SetMonsterImageUrls(monsters);
            SetItemImageUrls(items);
            SetExitNames(exits);
        }

        public List<Monster> UpdateMonsters(Room cr)
        {
            monsters = cr.GetMonstersOb();
            return monsters;
        }
        public List<Item> UpdateRoomItems(Room cr)
        {
            items = cr.GetItemsOb();
            return items;
        }
        public List<Exit> UpdateRoomExits(Room cr)
        {
            exits = cr.GetExitsOb();
            return exits;
        }

        public List<string> SetMonsterImageUrls(List<Monster> monsters)
        {
            foreach (Monster m in monsters) { monsterUrls.Add("~/images/" + m.Name + ".png"); }
            return monsterUrls;
        }
        public List<string> SetItemImageUrls(List<Item> items)
        {
            foreach (Item i in items) { itemUrls.Add("~/images/" + i.Name + ".png"); }
            return itemUrls;
        }
        public List<string> SetExitNames(List<Exit> exits)
        {
            foreach (Exit e in exits) { exitNames.Add(e.Direction); }
            return exitNames;
        }

        public string SetBackgroundState(Room cr)
        {
            roomUrl = "~/images/" + cr.Name + ".png";
            return roomUrl;
        }


        public string SetRHandUrl(string condition)
        {
            switch (condition)
            {
                case "sword":
                    return "~/images/rhand_sword.png";
                case "lantern":
                    return "~/images/rhand_lantern.png";
                case "fist":
                    return "~/images/rhand_fist.png";
                default:
                    return "~/images/rhand.png";
            }
        }

        public string SetLHandUrl(string condition)
        {
            switch (condition)
            {
                case "fist":
                    return "~/images/lhand_fist.png";
                default:
                    return "~/images/lhand.png";
            }
        }
    }
}
