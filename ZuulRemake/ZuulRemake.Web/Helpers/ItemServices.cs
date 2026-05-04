using ZuulRemake.Classes;
using ZuulRemake.Web.Models;

namespace ZuulRemake.Web.Helpers
{
    public class ItemServices
    {
        // ---------------------------------------------------------------
        // USE DISPATCH
        // ---------------------------------------------------------------

        public GameState UseItem(GameState state, string itemName)
        {
            return itemName.ToLower() switch
            {
                "lantern" => ApplyLantern(state),
                "key" => ApplyKey(state),
                "armour" => UseArmour(state),
                "potion" => UsePotion(state),
                "sword" => UseSword(state),
                _ => UnknownItem(state, itemName)
            };
        }

        // ---------------------------------------------------------------
        // USE METHODS
        // ---------------------------------------------------------------

        public bool UseLantern(Room currentRoom)
        {
            if (currentRoom.IsDark)
            {
                currentRoom.ToggleIsDark();
                return true;
            }
            return false;
        }

        public bool UseKey(Exit targetExit)
        {
            if (targetExit.IsLocked)
            {
                targetExit.Unlock();
                return true;
            }
            return false;
        }

        public GameState UseArmour(GameState state)
        {
            var item = state.player.GetItem("armour");
            state.messages.Clear();

            if (item == null)
            {
                state.AddMessage("You don't have any armour.");
                return state;
            }

            state.player.LevelUp(10);
            state.player.RemoveItem(item);
            state.AddMessage($"You equipped the armour and leveled up to {state.player.Level}.");
            return state;
        }

        public GameState UsePotion(GameState state)
        {
            var item = state.player.GetItem("potion");
            state.messages.Clear();

            if (item == null)
            {
                state.AddMessage("You don't have a potion.");
                return state;
            }

            state.player.AddHP(20);
            state.player.RemoveItem(item);
            state.AddMessage($"You drank a healing potion and restored yourself to {state.player.HP} HP!");
            return state;
        }

        public GameState UseSword(GameState state)
        {
            var item = state.player.GetItem("sword");
            state.messages.Clear();

            if (item == null)
            {
                state.AddMessage("You don't have a sword.");
                return state;
            }

            state.swordHeld = !state.swordHeld;
            state.AddMessage(state.swordHeld ? "You draw your sword." : "You sheathe your sword.");
            return state;
        }

        // ---------------------------------------------------------------
        // FACTORY
        // ---------------------------------------------------------------

        public Item CreateItemByName(string name)
        {
            return name.ToLower() switch
            {
                "potion" => new Item("potion", "A healing potion", 1, 20),
                "sword" => new Item("sword", "A sharp sword", 1, 0),
                "lantern" => new Item("lantern", "A bright lantern", 1, 0),
                "armour" => new Item("armour", "Heavy armour", 1, 10),
                "key" => new Item("key", "An old iron key", 0, 0),
                _ => new Item(name, "Unknown item", 1, 0)
            };
        }

        // ---------------------------------------------------------------
        // PRIVATE HELPERS
        // ---------------------------------------------------------------

        private GameState ApplyLantern(GameState state)
        {
            var item = state.player.GetItem("lantern");
            state.messages.Clear();

            if (item == null)
            {
                state.AddMessage("You don't have a lantern.");
                return state;
            }

            bool toggled = UseLantern(state.currentRoom);
            state.AddMessage(toggled
                ? "You light the lantern, and now you can see."
                : "The room is already lit.");
            return state;
        }

        private GameState ApplyKey(GameState state)
        {
            var item = state.player.GetItem("key");
            state.messages.Clear();

            if (item == null)
            {
                state.AddMessage("You don't have a key.");
                return state;
            }

            var southExit = state.currentRoom?.GetExit("south");

            if (southExit == null)
            {
                state.AddMessage("There's no locked exit to use the key on here.");
                return state;
            }

            bool unlocked = UseKey(southExit);
            if (unlocked)
            {
                state.UnlockedExits.Add($"{state.currentRoom.Name}:south");
                state.AddMessage("You use the key and unlock the exit.");
            }
            return state;
        }

        private GameState UnknownItem(GameState state, string itemName)
        {
            state.messages.Clear();
            state.AddMessage($"You can't use {itemName} here.");
            return state;
        }
    }
}