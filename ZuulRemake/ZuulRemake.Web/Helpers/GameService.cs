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
    }


    public class GameService : IGameService
    {
        public ILogger<GameService> _logger;
        public ItemService _itemService;

        public GameService(ILogger<GameService> logger, ItemService itemService)
        {
            _logger = logger;
            _itemService = itemService;
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
            var item = state.GetRoomItem(itemName);

            if (item == null)
            {
                state.messages.Clear();
                state.messages.Add("Item not found.");
            }
            else if (state.player.AddItem(item))
            {
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
            var player = state.player;
            var item = player.GetItem(itemName);

            if (item == null)
            {
                state.messages.Clear();
                state.messages.Add("Item not found.");
                return state;
            }

            else if (_itemService.CanUseItem(item, player))
            {
                string name = item.Name.ToLowerInvariant();               

                // SHOULD WE BE GETTING BOOLS FOR ALL OF THESE?
                // OR LET THEM MODIFY STATE AND RETURN STATE?
                switch (name)
                {
                    case "lantern":
                        if (_itemService.UseLantern(item, state.player))
                        // ^ we provide the lantern and player
                        // var room = player.currentRoom
                        // check isLit value (default is true)
                        // if false (dark), change isLit to true, then return true
                        // otherwise return false
                        {
                            // update state, change background image of room to lit version
                            state.messages.Clear();
                            state.messages.Add($"You used the {name}; now you can see!");
                        }
                        else
                        {
                            state.messages.Clear();
                            state.messages.Add($"This room is already lit. No need for the {name} here.");
                        }
                        break;

                    case "armour":
                        if (_itemService.UseArmour(item, state.player))
                        // ^ we provide the armour & player
                        // int buff = armour.StatIncrease
                        // if buff != null, return true
                        // else return false 
                        {
                            // update state
                            state.messages.Clear();
                            state.messages.Add($"You used the {name}; your health has increased by {item.StatIncrease}");
                        }                        
                        break;

                    case "sword":
                        _itemService.UseSword(item, player);
                        state.messages.Clear();
                        state.messages.Add($"You used the {name}.");
                        break;

                    case "potion":
                        _itemService.UsePotion(1, 2);
                        state.messages.Clear();
                        state.messages.Add($"You used the {name}.");
                        break;

                    case "ring":
                        _itemService.UseRing(1, 2);
                        state.messages.Clear();
                        state.messages.Add($"You used the {name}.");
                        break;

                    default:
                        state.messages.Add("Unknown item.");
                        break;
                }                
            }
            else
                {
                    state.messages.Clear();
                    state.messages.Add("You can't use this item here.");
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
                    .ToList()
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

            foreach (var itemName in save.InventoryItemNames)
            {
                var item = room.GetItem(itemName);
                if (item != null) state.player.AddItem(item);
            }

            return state;
        }
    }
}
