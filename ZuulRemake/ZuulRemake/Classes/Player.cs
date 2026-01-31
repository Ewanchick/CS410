using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZuulRemake.Classes
{
    internal class Player
    {
        public string Name { get; set; } = "?";
        public int HP { get; set; } = 100;
        public int CarryWeight { get; set; } = 0;
        public int MaxWeight { get; set; } = 2;
        public int Level { get; set; } = 10;

        private Room CurrentRoom { get; set; }
        private Room? chargeRoom { get; set; }

        private readonly BackPack backpack = new BackPack();
        private readonly int maximumWeight = 2;
        private readonly Stack<Room> previousRoom = new Stack<Room>();

        public Player(string name)
        {
            Name = name;
        }


        public string ExitsAvailable()
        {
            string returnString = "";
            returnString += CurrentRoom.GetExitString();
            return returnString;
        }

        /**
         * returns the current room that you are in.
         */
        public Room GetCurrentRoom()
        {
            return CurrentRoom;
        }

        /**
         * takes item out of room and places it into the backpack as long as
         * you can carry it.
         */
        public bool AddToBackPack(Item item)
        {
            if (item.Weight + CarryWeight <= MaxWeight)
            {
                CurrentRoom.RemoveItem(item.Name);
                backpack.AddItem(item);
                return true;
            }
            else
            {
                return false;
            }
        }

        /**
         * this returns the current room that you are in.
         */
        public string EnterRoom(Room nextRoom)
        {
            string returnString = "";
            CurrentRoom = nextRoom;

            returnString += CurrentRoom.ToString();
            return returnString;
        }

        /**
         * if the next room isnt empty or locked it takes you to the next room using
         * commandword go and the direction you want to go to depending on the 
         * given exits. then it pushes the room in the stack.
         */
        public string GoNewRoom(string direction)
        {
            if (!CurrentRoom.TryGetExit(direction, out Room nextRoom)) return "there is no door (or it is locked).";
            // Try to leave current room.
            previousRoom.Push(CurrentRoom);
            CurrentRoom = nextRoom;
            return CurrentRoom.ToString();           
        }

        /**
         * checks player inventory for key
         
        public bool checkForKey()
        {
            return backpack.keyCheck();
        }
        **/
        /**
         * displays the toString from the room class.
         */
        public string GetRoomDescription()
        {
            return CurrentRoom.ToString();
        }

        /**
         * if the player is able to add the item to the backpack then the player
         * will take the item. if not the game will tell the player it is too
         * heavy.
         */
        // Player should not be able to type in name of item if not in room
        // Make yes or no prompt instead
        public string TakeItem(string name)
        {
            string returnString = "";
            Item item = CurrentRoom.GetItem(name);
            if (item == null)
            {
                returnString += "that item isnt in the room";
            }
            else
            {
                if (AddToBackPack(item))
                {
                    returnString += "took: " + item.ToString();
                }
                else
                {
                    returnString += name + " is too heavy to carry";
                }
            }
            return returnString;
        }

        /**
         * checks to see if the item is in the backpack, if the backpack is
         * empty, it will tell you, otherwise it will remove the item from
         * the backpack and add it to the room.
         */
        public string DropItem(string name)
        {
            string returnString = "";
            Item itemRemove = GetItemFromBackpack(name);

            if (itemRemove == null)
            {
                returnString += "this item isnt in your backpack";
            }
            else
            {
                RemoveFromBackpack(name);
                CurrentRoom.SetItem(name, itemRemove);
                returnString += name + " dropped";
            }
            return returnString;
        }

        /**
         * removes the item from the backpack.
         */
        public void RemoveFromBackpack(string itemRemove)
        {
            backpack.RemoveItem(itemRemove);
        }

        /**
         * returns an item from the backpack if it is available.
         */
        public Item GetItemFromBackpack(string item)
        {
            return backpack.GetItem(item);
        }

        /**
         * displays player HP
         */
        public int GetHP()
        {
            return HP;
        }


        //public void setCurrentRoom(Room newRoom)
        //{
        //    previousRoom.push(CurrentRoom);
        //    CurrentRoom = newRoom;
        //}

        /**
         * takes the player back one room using the stack utilit. if there
         * is no previous room it will tell you that you cant go back.
         * 
         */
        public string GoBack()
        {
            string returnString = "";

            if (previousRoom.Count == 0)
            {
                returnString += "There is no turning back!";
            }
            else
            {
                Room lastRoom = previousRoom.Pop();
                EnterRoom(lastRoom);
                returnString += CurrentRoom.ToString();
            }
            return returnString;
        }

        /**
         * if the weight of the backpack is less than the maximum weight
         * and the item is less than the remaining weight available
         * then the player can pick up the item. if not the player is unable
         * to.
         */
        private bool CanCarry(Item item)
        {
            bool canCarry = true;
            int totalWeight = backpack.GetTotalWeight() + item.Weight;
            if (totalWeight > maximumWeight)
            {
                canCarry = false;
            }
            return canCarry;
        }

        public void EquipItem()
        {
            string returnString = "";
            HP += 101;
            returnString += HP;
        }

        /**
         * Reduces HP of the Player based on damage taken.
         */
        public int TakeDamage(int damage)
        {
            HP -= damage;
            if (HP < 0) HP = 0;
            return HP;
        }

        /**
         * Increases Player Level (damage dealt)
         */
        public void LevelUp(int levels)
        {
            Level += levels;
        }

        /**
         * Deals damage to a monster based on current level
         */
        public int DealAttack(Monster monster)
        {            
            return Level;
        }


        // simplified ^
        
        // Player level determines damage dealt to monster
        // All player does is deal damage
        // weapons increase level


        // MOVE LOTS OF THIS LOGIC TO GAME CLASS
        public string attack(string name)
        {
            string returnString = "";
            Monster monster = CurrentRoom.GetMonster(name);
            if (monster == null)
            {
                returnString += "that monster is no monster in this room";
            }
            else if (GetInventoryString().Contains("sword"))
            {
                HP -= 100;
                returnString += "\nyou attacked the monster" + "\nmonster HP: " + monster.HP;
                returnString += "\nthe monster hit you back";
                monster.TakeDamage();
                returnString += "\n" + "your HP: " + HP;
            }
            else
            {
                returnString += " you dont have anything to attack" + name + "with";
            }

            return returnString;
        }

        // MOVE TO GAME CLASS
        /**
         * Ends the game if player HP reaches 0
         */
        public bool gameOver()
        {
            return HP == 0;
        }

        /**
         * Returns items in the inventory
         */
        public string GetInventoryString()
        {
            int totalWeight = backpack.GetTotalWeight();
            return backpack.InventoryToString() + "\nweight: " + totalWeight + "/" + maximumWeight + "\nHP:" + HP;
        }

        /**
         * Charges the beamer to memorize the current room
         */
        public string BeamerCharge()
        {
            string returnstring = "";
            chargeRoom = CurrentRoom;
            returnstring += "charged beamer";
            return returnstring;
        }

        /**
         * fires the beamer to take you to the charge room
         */
        public string BeamerFire()
        {
            string returnstring = "";
            if (chargeRoom != null)
            {
                EnterRoom(chargeRoom);
                returnstring += "fired beamer:" + "\n" + GetRoomDescription();
            }
            else
            {
                returnstring += "you have to charge the beamer first";
            }
            return returnstring;
        }
    }
}
