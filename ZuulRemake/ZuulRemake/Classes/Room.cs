using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZuulRemake.Classes
{
    /**
     * This class represents a Room to which the Player can travel. A Room has a Name, 
     * ShortDescriptions, and LongDescriptions. A Room can contain Items and Monsters, 
     * and also contains a collection of Exits which are associated with directions.
     */
    public class Room
    {
        // Backwards-compatible single-description field (used by older WorldBuilder/tests)
        private readonly string? description;

        // New refactor properties
        public string Name { get; }
        public string ShortDescription { get; }
        public string LongDescription { get; }

        // Internal stores (case-insensitive keys)
        private readonly Dictionary<string, Exit> exits = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, Item> items = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, Monster> monsters = new(StringComparer.OrdinalIgnoreCase);

        // Old-style ctor (keeps existing WorldBuilder/tests working)
        public Room(string description)
        {
            this.description = description ?? throw new ArgumentNullException(nameof(description));
            Name = string.Empty;
            ShortDescription = description;
            LongDescription = string.Empty;
        }

        // New refactor ctor
        public Room(string name, string shortDescription, string longDescription)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ShortDescription = shortDescription ?? throw new ArgumentNullException(nameof(shortDescription));
            LongDescription = longDescription ?? throw new ArgumentNullException(nameof(longDescription));
            description = shortDescription;
        }

        /* ------------------------------ INFORMATION ------------------------------ */

        public override string ToString()
        {
            // Preserve a human-friendly description used in tests
            var shortDesc = GetShortDescription();
            if (!string.IsNullOrWhiteSpace(Name))
                return $"You are standing in the {Name}: {shortDesc}";
            return $"You are {shortDesc}.";
        }

        public string GetLongDescription()
        {
            // Keep previous style: "You are ..." + exits/items/monsters
            var sb = new StringBuilder();
            sb.AppendLine(ToString());
            sb.AppendLine(GetRoomItems());
            sb.AppendLine(GetRoomMonsters());
            sb.Append(GetExitString());
            return sb.ToString();
        }

        public string GetShortDescription() => !string.IsNullOrWhiteSpace(ShortDescription) ? ShortDescription : (description ?? string.Empty);

        public string GetExitString()
        {
            if (exits.Count == 0) return "Exits from this room:";
            var parts = exits.Values.Select(e => e.IsLocked ? $"{e.Direction} (locked)" : e.Direction);
            return "Exits from this room: " + string.Join(", ", parts);
        }

        public string GetRoomItems()
        {
            if (items.Count == 0) return "There are no items in this room!";
            return "Items in this room: " + string.Join(", ", items.Keys);
        }

        public string GetRoomMonsters()
        {
            if (monsters.Count == 0) return "There are no Monsters in this room!";
            return "Monsters in this room: " + string.Join(", ", monsters.Keys);
        }

        /* ----------------------- EXITS ----------------------- */

        // Backwards-compatible SetExit API used elsewhere in the codebase/tests
        public void SetExit(string direction, Room neighbor)
            => SetExit(direction, neighbor, false);

        public void SetExit(string direction, Room neighbor, bool isLocked)
        {
            if (string.IsNullOrWhiteSpace(direction)) throw new ArgumentException("direction required");
            if (neighbor == null) throw new ArgumentNullException(nameof(neighbor));
            var dir = direction.ToLowerInvariant();
            exits[dir] = new Exit(dir, neighbor, isLocked);
        }

        // New-style alias kept for refactor callers
        public void AddExit(string direction, Room targetRoom, bool isLocked = false) => SetExit(direction, targetRoom, isLocked);

        public bool IsExitLocked(string direction)
        {
            if (string.IsNullOrWhiteSpace(direction)) return false;
            return exits.TryGetValue(direction, out var e) && e.IsLocked;
        }

        // TryGetExit used by tests/helpers — respects lock state
        public bool TryGetExit(string direction, out Room? nextRoom)
        {
            nextRoom = null;
            if (string.IsNullOrWhiteSpace(direction)) return false;
            if (!exits.TryGetValue(direction, out var e)) return false;
            if (e.IsLocked) return false;
            nextRoom = e.TargetRoom;
            return true;
        }

        // Return raw Exit (nullable)
        public Exit? GetExitRaw(string direction)
        {
            if (string.IsNullOrWhiteSpace(direction)) return null;
            exits.TryGetValue(direction, out var e);
            return e;
        }

        /* ----------------------- ITEMS ----------------------- */

        // Backwards-compatible SetItem(name, item)
        public void SetItem(string itemName, Item item)
        {
            if (string.IsNullOrWhiteSpace(itemName)) throw new ArgumentException("itemName required");
            if (item == null) throw new ArgumentNullException(nameof(item));
            items[itemName] = item;
        }

        // Additional convenience matching refactor call-sites
        public void AddItem(Item item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            items[item.Name] = item;
        }

        // Remove by name (compatibility with Player.Remove usage)
        public void RemoveItem(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return;
            items.Remove(name);
        }

        // Remove by instance
        public void RemoveItem(Item item)
        {
            if (item == null) return;
            RemoveItem(item.Name);
        }

        // Return null if not found (compatibility)
        public Item? GetItem(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            items.TryGetValue(name, out var item);
            return item;
        }

        public Item AddItem(string name, Item item)
        {
            SetItem(name, item);
            return item;
        }

        /* ---------------------- MONSTERS ---------------------- */

        public void SetMonster(string name, Monster monster)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("name required");
            if (monster == null) throw new ArgumentNullException(nameof(monster));
            monsters[name] = monster;
        }

        public Monster? GetMonster(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            monsters.TryGetValue(name, out var m);
            return m;
        }

        public bool RemoveMonster(string name) => !string.IsNullOrWhiteSpace(name) && monsters.Remove(name);

        // New-style helpers
        public void AddMonster(Monster monster)
        {
            if (monster == null) throw new ArgumentNullException(nameof(monster));
            monsters[monster.Name] = monster;
        }

        public void RemoveMonster(Monster monster)
        {
            if (monster == null) throw new ArgumentNullException(nameof(monster));
            monsters.Remove(monster.Name);
        }
    }
}