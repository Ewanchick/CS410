using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZuulRemake.Classes
{
    /** 
     * This is a nested class for Exit objects. Exits are properties of a Room and are associated with 
     * a direction and the rooms they connect to. Exits may be locked, and can be unlocked with a key.
     */
    public class Exit
    {
        public string Direction { get; }
        public Room TargetRoom { get; }
        public bool IsLocked { get; private set; }

        public Exit(string direction, Room targetRoom, bool isLocked = false)
        {
            Direction = direction;
            TargetRoom = targetRoom;
            IsLocked = isLocked;
        }

        public void Unlock()
        {
            IsLocked = false;
        }
    }
}
