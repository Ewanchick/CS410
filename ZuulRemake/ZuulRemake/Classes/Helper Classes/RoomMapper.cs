using System;
using System.Collections.Generic;
using System.Reflection;
using ZuulRemake.Models;

namespace ZuulRemake.Classes
{
    /// <summary>
    /// Maps domain Room, Exit, Item, and Monster objects to entity objects for persistence.
    /// </summary>
    internal static class RoomMapper
    {
        private static readonly Dictionary<string, int> RoomNameToId = [];

        public static Model.RoomEntity MapToEntity(Room room)
        {
            if (room == null) throw new ArgumentNullException(nameof(room));

            var itemsField = typeof(Room).GetField("Items", BindingFlags.NonPublic | BindingFlags.Instance);
            var monstersField = typeof(Room).GetField("Monsters", BindingFlags.NonPublic | BindingFlags.Instance);
            var exitsField = typeof(Room).GetField("Exits", BindingFlags.NonPublic | BindingFlags.Instance);

            var items = (List<Item>?)itemsField?.GetValue(room) ?? new List<Item>();
            var monsters = (List<Monster>?)monstersField?.GetValue(room) ?? new List<Monster>();
            var exits = (List<Exit>?)exitsField?.GetValue(room) ?? new List<Exit>();

            return new Model.RoomEntity
            {
                Name = room.Name,
                NarrativeDescription = room.NarrativeDescription,
                LongDescription = room.LongDescription,
                Items = items.ConvertAll(MapItemToEntity),
                Monsters = monsters.ConvertAll(MapMonsterToEntity),
                Exits = exits.ConvertAll(e => MapExitToEntity(e, room.Name))
            };
        }

        private static Model.ItemEntity MapItemToEntity(Item item)
        {
            if (item == null) return null!;

            return new Model.ItemEntity
            {
                Name = item.Name,
                Description = item.Description,
                Weight = item.Weight
            };
        }

        private static Model.MonsterEntity MapMonsterToEntity(Monster monster)
        {
            if (monster == null) return null!;

            return new Model.MonsterEntity
            {
                Name = monster.Name,
                HP = monster.HP,
                Level = monster.Level
            };
        }

        private static Model.ExitEntity MapExitToEntity(Exit exit, string currentRoomName)
        {
            var entity = new Model.ExitEntity
            {
                Direction = exit.Direction,
                IsLocked = exit.IsLocked,  // Read the IsLocked property value
                                           // RoomId will be set after the room is saved to the database
                RoomId = GetRoomIdByName(currentRoomName)
            };
            return entity;
        }
        /// <summary>
        /// Store the mapping after a room is saved to the database.
        /// Call this after SaveChanges() to link domain object names to DB IDs.
        /// </summary>
        public static void RegisterRoomId(string roomName, int databaseId)
        {
            RoomNameToId[roomName] = databaseId;
        }

        /// <summary>
        /// Retrieve the database ID for a room by its name.
        /// Returns 0 if not yet saved.
        /// </summary>
        public static int GetRoomIdByName(string roomName)
        {
            return RoomNameToId.TryGetValue(roomName, out var id) ? id : 0;
        }

        /// <summary>
        /// Convert a saved RoomEntity back to a domain Room object.
        /// </summary>
        public static Room MapToDomain(Model.RoomEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var room = new Room(entity.Name ?? "", entity.NarrativeDescription ?? "", entity.LongDescription ?? "");

            // Track this room's ID for exit mappings
            if (entity.Id > 0)
            {
                RegisterRoomId(entity.Name ?? "", entity.Id);
            }

            return room;
        }
    }
}