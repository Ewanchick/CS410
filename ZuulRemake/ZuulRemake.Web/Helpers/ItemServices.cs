using ZuulRemake.Classes;
using System.Linq;
namespace ZuulRemake.Web.Helpers
{

    public class ItemServices
    {
        //Needs to impliment the ability to show or hide items within the UI that calls this method
        public bool UseLantern(Room currentRoom)
        {
            if(currentRoom.isDark == true)
            {
                currentRoom.ToggleIsDark();
                return true;
            }
            else
            {
                return false;
            }
            
        }
        public bool UseKey(Exit targetExit)
        {
            if(targetExit.IsLocked == true)
            {
                targetExit.Unlock();
                return true;
            }
            else
            {
                return false;
            }
        }
        public void UseArmor(Player player)
        {
           
        }

    }
}
