using System;
using ZuulRemake.Classes;
using ZuulRemake.Repos;

namespace ZuulRemake.Services
{
    /// <summary>
    /// Encapsulates all game logic, independent of UI (console or web).
    /// </summary>
    public class GameService
    {
        private readonly Player player;
        private readonly RoomRepo roomRepo;
        private readonly Room startingRoom;

        public GameService(RoomRepo roomRepo, Player player)
        {
            this.roomRepo = roomRepo ?? throw new ArgumentNullException(nameof(roomRepo));
            this.player = player ?? throw new ArgumentNullException(nameof(player));

            // Load starting room from DB or initialize
            this.startingRoom = GetRoomByName("Entryway") ?? InitializeWorld();
            player.GoNewRoom(this.startingRoom);
        }

        // Game State
        public Player Player => player;
        public Room CurrentRoom => player.GetCurrentRoom();
        public bool IsGameOver => !player.IsAlive || CurrentRoom?.Name == "Exit";

        // Game Actions
        public string Move(string direction)
        {
            var exit = CurrentRoom?.GetExit(direction);
            if (exit == null)
                return "You can't go that way.";

            if (exit.IsLocked)
                return "That exit is locked.";

            player.GoNewRoom(exit.TargetRoom);
            return GetRoomDescription();
        }

        public string GetRoomDescription()
        {
            return CurrentRoom?.GetLongDescription() ?? "You are nowhere.";
        }

        public string TakeItem(string itemName)
        {
            var item = CurrentRoom?.GetItem(itemName);
            if (item == null)
                return $"There is no '{itemName}' here.";

            if (player.AddItem(item))
            {
                CurrentRoom?.RemoveItem(item);
                return $"You took the {itemName}.";
            }

            return "You can't carry that.";
        }

        public string DropItem(string itemName)
        {
            var item = player.GetItem(itemName);
            if (item == null)
                return $"You don't have a '{itemName}'.";

            player.RemoveItem(item);
            CurrentRoom?.AddItem(item);
            return $"You dropped the {itemName}.";
        }

        public string GetInventory()
        {
            return player.ReadInventory();
        }

        public string GetHelp()
        {
            return "Available commands: go [direction], take [item], drop [item], inventory, help, quit";
        }

        // Helper methods
        private Room? GetRoomByName(string name)
        {
            var roomEntity = roomRepo.GetByName(name);
            if (roomEntity == null)
                return null;

            // Convert entity back to domain Room
            var room = new Room(
                roomEntity.Name ?? "Unknown",
                roomEntity.NarrativeDescription ?? "",
                roomEntity.LongDescription ?? "");

            return room;
        }

        private Room InitializeWorld()
        {
            // Build the world and capture all rooms
            WorldBuilder.Build(
                out var entryway,
                out var dininghall,
                out var ballroom,
                out var kitchen,
                out var bathroom,
                out var dungeon,
                out var bedroom,
                out var exit);

            // put all rooms in a list
            var allRooms = new[] { entryway, dininghall, ballroom, kitchen, bathroom, dungeon, bedroom, exit };

            // Convert domain Rooms to entities and save to DB
            var roomEntities = allRooms
                .Select(RoomMapper.MapToEntity)
                .ToList();

            roomRepo.AddRange(roomEntities);
            return entryway;
        }

        
    }
}