using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZuulRemake.Classes
{
    /// <summary>
    /// Thrown when a player attempts to go back but has no room history to return to.
    /// </summary>
    public class NoRoomHistoryException : Exception
    {
        public NoRoomHistoryException()
            : base("You haven't been anywhere yet — there's no room to go back to.") { }

        public NoRoomHistoryException(string message)
            : base(message) { }

        public NoRoomHistoryException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    /// <summary>
    /// Thrown when an operation requires the player to be in a room, but no room is set.
    /// </summary>
    public class NoCurrentRoomException : Exception
    {
        public NoCurrentRoomException()
            : base("The player is not currently in any room.") { }

        public NoCurrentRoomException(string message)
            : base(message) { }
    }

    /// <summary>
    /// Thrown when a player tries to pick up an item but cannot carry any more weight.
    /// </summary>
    public class InventoryFullException : Exception
    {
        public string ItemName { get; }

        public InventoryFullException(string itemName)
            : base($"You cannot carry '{itemName}' — you are already carrying too much weight.")
        {
            ItemName = itemName ?? "unknown";
        }

        public InventoryFullException(string itemName, string message)
            : base(message)
        {
            ItemName = itemName ?? "unknown";
        }
    }

    /// <summary>
    /// Thrown when a requested item is not found in a room or inventory.
    /// </summary>
    public class ItemNotFoundException : Exception
    {
        public string ItemName { get; }

        public ItemNotFoundException(string itemName)
            : base($"Item '{itemName}' was not found.")
        {
            ItemName = itemName ?? "unknown";
        }
    }

    /// <summary>
    /// Thrown when a requested monster is not found in the current room.
    /// </summary>
    public class MonsterNotFoundException : Exception
    {
        public string MonsterName { get; }

        public MonsterNotFoundException(string monsterName)
            : base($"There is no '{monsterName}' in this room.")
        {
            MonsterName = monsterName ?? "unknown";
        }
    }
}
