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

        public List<Monster> monsters = new();
        public List<Item> items = new();
        public List<Exit>? exits = new();

        public List<string> monsterUrls = new();
        public List<string> itemUrls = new();
        public List<string> exitNames = new();


        public GameState(Player p)
        {
            player = p;
            currentRoom = player.CurrentRoom;
            UpdateRoomObjects(currentRoom);

            roomUrl = SetBackgroundState(currentRoom);
            SetMonsterImageUrls(monsters);
            SetItemImageUrls(items);
            SetExitNames(exits);
        }

        public void UpdateGameState()
        {
            UpdateRoomObjects(currentRoom);
        }

        // OBJECT HANDLING 
        public void UpdateRoomObjects(Room cr)
        {
            monsters = cr.GetMonstersOb();
            items = cr.GetItemsOb();
            exits = cr.GetExitsOb();
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

        // VISUAL HANDLING
        public void UpdateAllImages(GameState gs)
        {
            if (gs.monsters != null) { SetMonsterImageUrls(gs.monsters); }
            if (gs.items != null) { SetItemImageUrls(gs.items); }
            SetBackgroundState(gs.currentRoom);
        }

        public List<string> SetExitNames(List<Exit> exits)
        {
            foreach (Exit e in exits) { exitNames.Add(e.Direction); }
            return exitNames;
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
