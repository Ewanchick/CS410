using System;
using System.Collections.Generic;
using System.Linq;
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
         * attempt movement, 
         * validate direction, 
         * check locked state, 
         * possibly unlock doors (if player has key), 
         * tell Player to move if allowed
         */

        public string MovePlayer(Player player, string direction)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));
            if (string.IsNullOrWhiteSpace(direction)) return "Go where?";

            Room? currentRoom = player.CurrentRoom;
            Exit? exit = currentRoom.GetExit(direction);

            if (exit == null) return "There is no exit that way.";

            if (exit.IsLocked) return "The door is locked! Use a key to unlock this door.";

            player.GoNewRoom(exit.TargetRoom);

            return $"You move {direction}.\n" +
                   $"{player.CurrentRoom.GetLongDescription()}";
        }
    }
}