using System.Xml.Linq;
using ZuulRemake.Classes;
using ZuulRemake.Services;
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
        private readonly ItemServices _itemServices;

        public GameService(ILogger<GameService> logger, ItemServices itemServices)
        {
            _logger = logger;
            _itemServices = itemServices;
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
                state.CollectedItemNames.Add(item.Name);
                state.messages.Clear();
                state.messages.Add($"The {item.Name} was added to your inventory.");
            }
            else
            {
                state.messages.Clear();
                state.messages.Add("You're carrying too many items.");
            }
            _logger.LogInformation("PickUpItem: itemName={Name}, CarryWeight={Carry}, MaxWeight={Max}, CanCarry={Can}",
    itemName, state.player.CarryWeight, state.player.MaxWeight, state.player.CanCarry(item));
            return state;
        }

        public GameState UseItem(GameState state, string itemName)
        {
            return _itemServices.UseItem(state, itemName);
        }

        public GameState Attack(GameState state, string target)
        {
            state.messages.Clear();

            if (state.currentRoom == null)
            {
                state.AddMessage("You're not in a room.");
                return state;
            }

            var p = state.player;
            var m = state.currentRoom.GetMonster(target);

            if (m == null)
            {
                state.AddMessage("There is no monster in this room.");
                return state;
            }

            if (!p.Inventory.Any(i => i.Name.Equals("sword", StringComparison.OrdinalIgnoreCase)))
            {
                state.AddMessage("You need a sword in order to attack.");
                return state;
            }

            _logger.LogInformation("Before attack: {Monster} HP={MHp}, Player HP={PHp}",
                m.Name, m.HP, p.HP);

            // Player attacks
            m.TakeDamage(p.Level);

            _logger.LogInformation("After player attack: {Monster} HP={MHp}, IsAlive={Alive}",
                m.Name, m.HP, m.IsAlive);

            if (!m.IsAlive)
            {
                var drop = m.Drop;
                if (drop != null)
                {
                    state.currentRoom.AddItem(drop);
                    state.AddMessage($"You have defeated the {m.Name}! It dropped a {drop.Name}.");
                }
                else
                {
                    state.AddMessage($"You have defeated the {m.Name}!");
                }

                state.currentRoom.RemoveMonster(m);
                state.DefeatedMonsterNames.Add(m.Name);
                state.MonsterHpStates.Remove(m.Name);
                return state;
            }

            // Monster retaliates
            p.TakeDamage(m.Level);
            state.MonsterHpStates[m.Name] = m.HP;
            if (!p.IsAlive)
            {
                state.AddMessage(
                    $"You attack the {m.Name} for {p.Level} damage, but the {m.Name} " +
                    $"strikes back for {m.Level} and you fall... YOU DIED.");
                return state;
            }

            state.AddMessage(
                $"You attack the {m.Name} for {p.Level} damage! " +
                $"The {m.Name} hits back for {m.Level} damage. " +
                $"({m.Name} HP: {m.HP}, your HP: {p.HP})");

            return state;
        }

        private Item CreateItemByName(string name)
        {
            return name.ToLower() switch
            {
                "potion" => new Item("Potion", "Use this to increase your health!", 1, 50),
                "sword" => new Item("Sword", "Heavy and sharp, capable of slaying the mightiest beast.", 1, 50),
                "lantern" => new Item("Lantern", "This should be able to light up any dark rooms.", 1, 0),
                "armour" => new Item("Armour", "Protect yourself from the lurking dangers!", 1, 20),
                "key" => new Item("Key", "This looks like it should fit the lock in the Entryway...", 0, 0),
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
                SwordHeld = state.swordHeld,
                RoomLit = state.roomLit,
                InventoryItemNames = state.player.Inventory.Select(i => i.Name).ToList(),
                CollectedItemNames = state.CollectedItemNames.ToList(),
                DefeatedMonsterNames = state.DefeatedMonsterNames.ToList(),
                MonsterHpStates = new Dictionary<string, int>(state.MonsterHpStates),
                UnlockedExits = state.UnlockedExits.ToList()

            };
        }

        public GameState LoadFromSave(GameSaveDto save)
        {
            _logger.LogInformation("LoadFromSave CollectedItemNames: {Items}",
    string.Join(", ", save.CollectedItemNames));
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
            state.swordHeld = save.SwordHeld; 
            state.roomLit = save.RoomLit;
            state.CollectedItemNames = save.CollectedItemNames.ToList();
            state.MonsterHpStates = new Dictionary<string, int>(save.MonsterHpStates);

            state.DefeatedMonsterNames = save.DefeatedMonsterNames.ToList();
            //Checks for defeated monsters
            foreach (var monsterName in save.DefeatedMonsterNames)
            {
                foreach (var r in allRooms)
                {
                    var mon = r.GetMonster(monsterName);
                    if (mon != null)
                    {
                        // If the monster had a drop and player hasn't collected it yet, restore it
                        if (mon.Drop != null && !save.CollectedItemNames
                                .Contains(mon.Drop.Name, StringComparer.OrdinalIgnoreCase))
                        {
                            r.AddItem(mon.Drop);
                        }
                        r.RemoveMonster(mon);
                        break;
                    }
                }
            }
            //Checks the existing Monsters
            foreach (var kvp in save.MonsterHpStates)
            {
                foreach (var r in allRooms)
                {
                    var mon = r.GetMonster(kvp.Key);
                    if (mon != null)
                    {
                        var damage = mon.HP - kvp.Value;
                        if (damage > 0) mon.TakeDamage(damage);
                        break;
                    }
                }
            }
            foreach (var unlockedExit in save.UnlockedExits)
            {
                var parts = unlockedExit.Split(':');
                if (parts.Length != 2) continue;
                var roomName = parts[0];
                var direction = parts[1];
                var targetRoom = allRooms.FirstOrDefault(r =>
                    r.Name.Equals(roomName, StringComparison.OrdinalIgnoreCase));
                targetRoom?.GetExit(direction)?.Unlock();
            }
            state.UnlockedExits = save.UnlockedExits.ToList();
            return state;
        }
    }
}
