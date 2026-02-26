using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZuulRemake.Classes
{
    /**
     * This class manages navigation between Rooms by a Player.
     */
    public class RoomTraversalManager
    {
        public Player player;
        public Room? currentRoom;
        public Stack<Room>? prevRooms;

        public RoomTraversalManager(Player p)
        {
            player = p;
            currentRoom = player.CurrentRoom;
            //prevRooms = player.PreviousRooms;
        }

        //public Room GoNewRoom(string direction)
        //{
        //    if ()
        //    {

        //    }
        //}
    }
}