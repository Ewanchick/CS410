using System;
using ZuulRemake.Classes;
using ZuulRemake.Repos;
using ZuulRemake.Models;

namespace ZuulRemake.Services
{
    public class GameService
    {
        private readonly RoomRepo _roomRepo;
        private readonly Room startingRoom;

        public GameService(RoomRepo roomRepo)
        {
            _roomRepo = roomRepo;
        }

        public GameState StartNewGame(string playerName)
        {
            var Player = new Player(playerName);
            var entryway =  //GetOrBuildWorld();
            Player.GoNewRoom(entryway);
        }

        public string Move(GameState state, string direction)
        {
            var exit = state.CurrentRoom.GetExit(direction);
            if (exit == null) return "You can't go that way.";

            if (exit.IsLocked) return "That exit is locked.";

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