using System;
using System.Collections.Generic;
using System.Linq;

namespace ZuulRemake.Classes
{
    public class Player : Entity
    {
        public readonly Stack<Room> PreviousRooms = new Stack<Room>();
        private Room? ChargeRoom { get; set; }

        // Converted Inventory from ArrayList to strongly-typed List<Item>
        public List<Item> Inventory { get; protected set; }

        public int CarryWeight { get; private set; } = 0;
        public int MaxWeight { get; private set; } = 2;

        // Primary ctor used by tests / callers that only pass name
        public Player(string name) : base(name, hp: 100, level: 10)
        {
            Inventory = new List<Item>();
            Name = name;
        }

        // Overload used by Game (preserves compatibility with existing code)
        public Player(string name, int hp, int level) : base(name, hp, level)
        {
            Inventory = new List<Item>();
            Name = name;
        }

        // Compatibility helper used by Game (Game calls SetCurrentRoom)
        public void SetCurrentRoom(Room room) => CurrentRoom = room;

        /* ------------------------------ HP ------------------------------ */
        public int TakeDamage(int damage)
        {
            HP -= damage;
            if (HP < 0) HP = 0;
            return HP;
        }

        public void AddHP(int hp)
        {
            HP += hp;
        }

        /* ------------------------------ LEVEL ------------------------------ */
        public void LevelUp(int levels)
        {
            Level += levels;
        }

        /* ------------------------------ INVENTORY ------------------------------ */
        // Adds item to player's inventory after removing it from the room.
        public bool AddToInventory(Item item)
        {
            if (item == null) return false;
            if (item.Weight + GetTotalWeight() > MaxWeight) return false;

            // remove from current room (safe)
            CurrentRoom?.RemoveItem(item.Name);

            Inventory.Add(item);
            return true;
        }

        public bool PickUp(Item item) => AddToInventory(item);

        public void RemoveFromBackpack(string itemName)
        {
            var item = Inventory.FirstOrDefault(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
            if (item != null) Inventory.Remove(item);
        }

        // take item by name from current room and add to inventory
        public string TakeItem(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "that item isnt in the room";

            var item = CurrentRoom?.GetItem(name);
            if (item == null)
            {
                return "that item isnt in the room";
            }

            if (AddToInventory(item))
            {
                return "took: " + item.ToString();
            }
            return name + " is too heavy to carry";
        }

        /* ------------------------------ WEIGHT ------------------------------ */
        public void AddWeight(int weight) => CarryWeight += weight;
        public void RemoveWeight(int weight) => CarryWeight -= weight;
        public void AddMaxWeight(int weight) => MaxWeight += weight;
        public void RemoveMaxWeight(int weight) => MaxWeight -= weight;

        /* ------------------------------ ROOM NAVIGATION ------------------------------ */
        public Room GetCurrentRoom()
        {
            if (CurrentRoom == null) throw new InvalidOperationException("CurrentRoom is null.");
            return CurrentRoom;
        }

        public void GoNewRoom(Room newRoom)
        {
            if (CurrentRoom != null) PreviousRooms.Push(CurrentRoom);
            CurrentRoom = newRoom;
        }

        public bool CanGoBack() => PreviousRooms.Count > 0;

        public Room? GoBack()
        {
            if (!CanGoBack()) throw new InvalidOperationException("No previous rooms to go back to.");
            CurrentRoom = PreviousRooms.Pop();
            return CurrentRoom;
        }

        /* ------------------------------ BEAMER ------------------------------ */
        public void ChargeBeamer(Room room) => ChargeRoom = room;
        public bool CanFireBeamer() => ChargeRoom != null;
        public Room? FireBeamer()
        {
            if (!CanFireBeamer()) throw new InvalidOperationException("ChargeRoom is null.");
            GoNewRoom(ChargeRoom!);
            return CurrentRoom;
        }

        public string ExitsAvailable()
        {
            if (CurrentRoom == null) return "CurrentRoom is null. There are no exits, because you are not in a room!";
            return CurrentRoom.GetExitString();
        }

        /* ------------------------------ DROP / INVENTORY HELPERS ------------------------------ */
        public string DropItem(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "this item isnt in your Backpack";

            var item = GetItemFromInventory(name);
            if (item == null) return "this item isnt in your Backpack";

            // remove from inventory and place into room
            Inventory.Remove(item);
            CurrentRoom?.SetItem(name, item);
            return name + " dropped";
        }

        public Item? GetItemFromInventory(string itemName)
        {
            if (string.IsNullOrWhiteSpace(itemName)) return null;
            return Inventory.FirstOrDefault(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
        }

        private bool CanCarry(Item item) => (GetTotalWeight() + item.Weight) <= MaxWeight;

        public void EquipItem()
        {
            HP += 101;
        }

        public string attack(string name)
        {
            var monster = CurrentRoom?.GetMonster(name);
            if (monster == null) return "that monster is no monster in this room";

            if (GetInventoryString().Contains("sword"))
            {
                HP -= 100;
                monster.TakeDamage(50);
                return $"\nyou attacked the monster\nmonster HP: {monster.HP}\n the monster hit you back\nyour HP: {HP}";
            }

            return $" you dont have anything to attack {name} with";
        }

        public bool gameOver() => HP == 0;

        public string GetInventoryString()
        {
            var names = Inventory.Select(i => i.Name).ToArray();
            var list = names.Length == 0 ? "Backpack is empty" : string.Join(", ", names);
            return $"{list}\nweight: {GetTotalWeight()}/{MaxWeight}\nHP:{HP}";
        }

        public int GetTotalWeight() => Inventory.Sum(w => w.Weight);

        public string BeamerCharge()
        {
            ChargeRoom = CurrentRoom;
            return "charged beamer";
        }

        public string BeamerFire()
        {
            if (ChargeRoom != null)
            {
                EnterRoom(ChargeRoom);
                return "fired beamer:\n" + GetRoomDescription();
            }
            return "you have to charge the beamer first";
        }

        public string EnterRoom(Room nextRoom)
        {
            CurrentRoom = nextRoom;
            return CurrentRoom.ToString();
        }

        public string GetRoomDescription() => CurrentRoom?.ToString() ?? "You are nowhere.";
    }
}
