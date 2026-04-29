using ZuulRemake.Classes;
using ZuulRemake.Web.Models;

namespace ZuulRemake.Web.Helpers
{
    // placeholders for ethan w.'s stuff
    public interface IItemService
    {
        public bool CanUseItem(Item item, Player player);
        public bool UseSword(Item item, Player player);
        public bool UseArmour(Item item, Player player);
        public bool UseLantern(Item item, Player player);
        public bool UseRing(Item item, Player player);
        public bool UsePotion(Item item, Player player);
    }

    public class ItemService : IItemService
    {

        public bool CanUseItem(Item item, Player player)
        {
            return true;
        }
        public bool UseSword(Item item, Player player)
        {
            return true;
        }
        public bool UseArmour(Item item, Player player)
        {
            return true;
        }
        public bool UseLantern(Item item, Player player)
        {
            return true;
        }
        public bool UsePotion(Item item, Player player)
        {
            return true;
        }
        public bool UseRing(Item item, Player player)
        {
            return true;
        }
    }
}
