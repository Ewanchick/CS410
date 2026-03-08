using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ZuulRemake.Classes
{
    /**
     * This class manages navigation rules between Rooms by a Player.
     */
    public class NavigationManager
    {
        /**
         * Attempt to move a Player from their current room to a Room attached to a 
         * particular Exit. First, validate the direction, then check if the Exit is 
         * locked. If so, call HandleLockedDoor(). Otherwise, move the Player via 
         * GoNewRoom, providing it the targetRoom of the Exit.
         */
        public string MovePlayer(Player player, string direction)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));
            if (string.IsNullOrWhiteSpace(direction)) return "Go where?";

            Room? currentRoom = player.CurrentRoom;
            Exit? exit = currentRoom.GetExit(direction);
            Item? key = player.GetItem("key");

            if (exit == null) return "There is no exit that way.";

            if (exit.IsLocked)
            {
                return HandleLockedDoor(player);
            }
            else
            {
                player.GoNewRoom(exit.TargetRoom);

                return $"You move {direction}.\n" +
                       $"{player.CurrentRoom.GetLongDescription()}";
            }

        }

        /**
         * Handle prompting to a player when they encounter a locked 
         * door. If the Player already has a key, prompt them to use 
         * it, otheriwse, prompt them to go find a key.
         */
        public string HandleLockedDoor(Player player)
        {
            StringBuilder str = new StringBuilder("The door is locked! A key will unlock it.");
            Item? key = player.GetItem("key");

            if (key != null)
            {
                str.Append("You already have a key! Enter 'use key' to unlock the door and proceed. ");
            }
            else
            {
                str.Append("Come back and try again when you have found a key. ");
            }
            return str.ToString();
        }
    }
}