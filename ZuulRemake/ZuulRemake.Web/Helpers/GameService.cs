using System.Xml.Linq;
using ZuulRemake.Classes;
using ZuulRemake.Web.Models;

namespace ZuulRemake.Web.Helpers
{
    public interface IGameService
    {
        GameState CreateNewGame();
        GameState Move(GameState state, string direction);
        GameState PickUpItem(GameState state, string itemName);
        GameState Attack(GameState state, string target);
        GameSaveDto ToSaveDto(GameState state);
        GameState LoadFromSave(GameSaveDto save);
        GameState UseItem(GameState state, string itemName);
    }


    public class GameService : IGameService
    {
        public ILogger<GameService> _logger;

        public GameService(ILogger<GameService> logger)
        {
            _logger = logger;
        }
        
        public GameState CreateNewGame()
        {
            var player = new Player("Player");
            var game = new Game(player);
            var state = new GameState(player);
            state.EnterRoom(state.player.CurrentRoom);
            return state;
        }

        public GameState Move(GameState state, string direction)
        {
            var exit = state.currentRoom.GetExit(direction);

            if (exit == null)
            {
                state.messages.Clear();
                state.AddMessage("You can't go that way.");
                return state;
            }

            if (exit.IsLocked)
            {
                state.messages.Clear();
                state.AddMessage("That exit is locked.");
                return state;
            }

            state.player.GoNewRoom(exit.TargetRoom);
            state.EnterRoom(state.player.CurrentRoom);

            return state;
        }

        public GameState PickUpItem(GameState state, string itemName)
        {
            var item = state.items.FirstOrDefault(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));

            if (item == null)
            {
                state.messages.Clear();
                state.messages.Add("Item not found.");
            }
            else if (state.player.AddItem(item))
            {
                state.currentRoom.RemoveItem(item);
                state.CollectedItemNames.Add(itemName);
                state.messages.Clear();
                state.messages.Add($"The {item.Name} was added to your inventory.");
            }
            else
            {
                state.messages.Clear();
                state.messages.Add("You're carrying too many items.");
            }

            return state;
        }

        public GameState UseItem(GameState state, string itemName)
        {
            var item = state.player.GetItem(itemName);
            state.messages.Clear();

            if (item == null)
            {
                state.AddMessage("You don't have that item.");
                return state;
            }

            switch (itemName.ToLower())
            {
                case "potion":
                    state.player.AddHP(20);
                    state.player.RemoveItem(item);
                    state.AddMessage($"You drank a healing potion and healed yourself to {state.player.HP} HP!");
                    break;

                case "armour":
                    state.player.LevelUp(10);
                    state.player.RemoveItem(item);
                    state.AddMessage($"You equipped the armour and leveled up to {state.player.Level}.");
                    break;

                case "lantern":
                    state.roomLit = true;
                    state.AddMessage("You light the lantern, and now you can see.");
                    break;

                case "sword":
                    state.swordHeld = !state.swordHeld;
                    state.AddMessage(state.swordHeld ? "You draw your sword." : "You sheathe your sword.");
                    break;

                case "key":
                    // bad bad bad
                    state.currentRoom?.GetExit("south")?.Unlock();
                    state.AddMessage("You use the key and unlock the exit.");
                    break;
                default:
                    state.AddMessage($"You can't use {itemName} here.");
                    break;
            }

            return state;
        }

        public GameState Attack(GameState state, string target)
        {
            if (state.currentRoom == null) return state;
            var p = state.player;
            var m = state.currentRoom.GetMonster(target);

            if (m == null)
            {
                state.messages.Clear();
                state.messages.Add("There is no monster in this room.");
                return state;
            }

            if (!p.Inventory.Any(i => i.Name.Equals("sword", StringComparison.OrdinalIgnoreCase)))
            {
                state.messages.Clear();
                state.messages.Add("You need a sword in order to attack.");
                return state;
            }

            m.TakeDamage(p.Level);
            state.messages.Clear();
            state.messages.Add($"You attack the {m.Name} for {p.Level} damage!");

            if (!m.IsAlive)
            {
                var drop = m.Drop;
                if (drop != null) state.currentRoom.AddItem(drop);

                state.messages.Clear();
                state.messages.Add($"You have defeated the {m.Name}!");
                return state;
            }
            else
            {
                p.TakeDamage(m.Level);
                state.messages.Clear();
                state.messages.Add($"The {m.Name} attacks you for {m.Level} damage!");
                return state;
            }
        }

        private Item CreateItemByName(string name)
        {
            return name.ToLower() switch
            {
                "potion" => new Item("potion", "A healing potion", 1, 20),
                "sword" => new Item("sword", "A sharp sword", 1, 0),
                "lantern" => new Item("lantern", "A bright lantern", 1, 0),
                "armour" => new Item("armour", "Heavy armour", 1, 10),
                _ => new Item(name, "Unknown item", 1, 0)
            };
        }

        public GameSaveDto ToSaveDto(GameState state)
        {
            return new GameSaveDto
            {
                PlayerName = state.player.Name,
                HP = state.player.HP,
                Level = state.player.Level,
                MaxWeight = state.player.MaxWeight,
                CurrentRoomName = state.currentRoom.Name,
                InventoryItemNames = state.player.Inventory
                    .Select(i => i.Name)
                    .ToList(),
                CollectedItemNames = state.player.Inventory
                    .Select(i => i.Name)
                    .ToList(),
            };
        }

        public GameState LoadFromSave(GameSaveDto save)
        {
            var state = CreateNewGame();

            state.player.LoadSaveData(
                save.PlayerName,
                save.HP,
                save.Level,
                save.MaxWeight
            );

            Room entryway, dininghall, ballroom, kitchen, bathroom, dungeon, bedroom, exit;
            WorldBuilder.Build(out entryway, out dininghall, out ballroom, out kitchen, out bathroom, out dungeon, out bedroom, out exit);
            var allRooms = new List<Room> { entryway, dininghall, ballroom, kitchen, bathroom, dungeon, bedroom, exit };

            var room = save.CurrentRoomName.ToLower() switch
            {
                "entryway" => entryway,
                "dining hall" => dininghall,
                "ballroom" => ballroom,
                "kitchen" => kitchen,
                "bathroom" => bathroom,
                "dungeon" => dungeon,
                "bedroom" => bedroom,
                "exit" => exit,
                _ => entryway
            };

            state.player.GoNewRoom(room);
            state.EnterRoom(room);

            foreach (var itemName in save.CollectedItemNames)
            {
                foreach (var r in allRooms)
                {
                    var item = r.GetItem(itemName);
                    if (item != null) r.RemoveItem(item);
                }
            }

            foreach (var itemName in save.InventoryItemNames)
            {
                var item = CreateItemByName(itemName);
                state.player.AddItem(item);
            }
            
            return state;
        }
    }
}
