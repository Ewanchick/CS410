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
            return state; // stubby
        }

        public GameState UseItem(GameState state, string itemName)
        {
            return state; // stubbier
        }

        public GameState Attack(GameState state, string target)
        {
            return state; // stubbiest
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
                if (item != null)
                    state.player.AddItem(item);
            }

            return state;
        }
    }
}
