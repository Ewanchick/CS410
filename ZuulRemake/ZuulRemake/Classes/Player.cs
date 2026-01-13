using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZuulRemake.Classes
{
    internal class Player
    {
        // instance variables - replace the example below with your own
        private string name;
        private Room currentRoom;
        private BackPack backpack;
        private int maximumWeight;
        private Stack<Room> previousRoom;
        private int turns = 0;
        private int turnsLeft = 20;
        private Room chargeRoom;
        private int hp = 100;
        private Monster monster;
        /**
         * Constructor for objects of class Player
         */
        public Player(string name)
        {
            backpack = new BackPack();
            this.name = name;
            maximumWeight = 2;
            previousRoom = new Stack<Room>();
        }

        public string exitsAvailable()
        {
            string returnString = "";
            returnString += currentRoom.GetExitString();
            return returnString;
        }

        /**
         * returns the current room that you are in.
         */
        public Room getCurrentRoom()
        {
            return currentRoom;
        }

        /**
         * takes item out of room and places it into the backpack as long as
         * you can carry it.
         */
        public bool AddToBackPack(Item item)
        {
            if (CanCarry(item))
            {
                currentRoom.removeItem(item.getName());
                backpack.addItem(item);
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
            turns++;
            string returnString = "";
            currentRoom = nextRoom;

            returnString += currentRoom.ToString();
            return returnString;
        }

        /**
         * if the next room isnt empty or locked it takes you to the next room using
         * commandword go and the direction you want to go to depending on the 
         * given exits. then it pushes the room in the stack.
         */
        public string GoNewRoom(string direction)
        {
            // Try to leave current room.
            string returnString = "";
            Room nextRoom = currentRoom.getExit(direction);
            if (nextRoom == null)
            {
                returnString += "There is no door!";
            }

            else
            {
                previousRoom.Push(currentRoom);
                enterRoom(nextRoom);
                returnString += currentRoom.ToString();
            }
            return returnString;
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
            return currentRoom.ToString();
        }

        /**
         * if the player is able to add the item to the backpack then the player
         * will take the item. if not the game will tell the player it is too
         * heavy.
         */
        public string takeItem(string name)
        {
            string returnString = "";
            Item item = currentRoom.GetItem(name);
            if (item == null)
            {
                returnString += "that item isnt in the room";
            }
            else
            {
                if (AddToBackPack(item))
                {
                    returnString += "took: " + item;
                }
                else
                {
                    returnString += name + " is too heavy to carry";
                }
            }
            return returnString;
        }

        /**
         * if the player is able to eat the item then the player will increase their maximum weight.
         */
        public string eatCookie(string name)
        {
            Item cookie = backpack.getItem(name);

            string returnString = "";
            if (name == "cookie")
            {

                if (cookie == null)
                {
                    cookie = currentRoom.removeItem(name);
                    returnString += "could not eat" + cookie;
                }
                else
                {
                    maximumWeight++;
                    returnString += "you ate the cookie, you have become stronger";
                    removeFromBackpack(name);
                }
            }
            return returnString;
        }

        /**
         * checks to see if the item is in the backpack, if the backpack is
         * empty, it will tell you, otherwise it will remove the item from
         * the backpack and add it to the room.
         */
        public string dropItem(string name)
        {
            string returnString = "";
            Item itemRemove = getItemFromBackpack(name);

            if (itemRemove == null)
            {
                returnString += "this item isnt in your backpack";
            }
            else
            {
                removeFromBackpack(name);
                currentRoom.setItem(name, itemRemove);
                returnString += name + " dropped";
            }
            return returnString;
        }

        /**
         * removes the item from the backpack.
         */
        public void removeFromBackpack(string itemRemove)
        {
            backpack.removeItem(itemRemove);
        }

        /**
         * returns an item from the backpack if it is available.
         */
        public Item getItemFromBackpack(string item)
        {
            return backpack.getItem(item);
        }

        /**
         * displays player hp
         */
        public int getHp()
        {
            return hp;
        }
        //public void setCurrentRoom(Room newRoom)
        //{
        //    previousRoom.push(currentRoom);
        //    currentRoom = newRoom;
        //}

        /**
         * takes the player back one room using the stack utilit. if there
         * is no previous room it will tell you that you cant go back.
         * 
         */
        public string goBack()
        {
            string returnString = "";

            if (previousRoom == null)
            {
                returnString += "There is no turning back!";
            }
            else
            {
                Room lastRoom = previousRoom.Pop();
                enterRoom(lastRoom);
                returnString += currentRoom.ToString();
            }
            return returnString;
        }
        //public string getCookie()
        //{

        //}
        /**
         * if the weight of the backpack is less than the maximum weight
         * and the item is less than the remaining weight available
         * then the player can pick up the item. if not the player is unable
         * to.
         */
        private bool CanCarry(Item item)
        {
            bool canCarry = true;
            int totalWeight = backpack.getTotalWeight() + item.getWeight();
            if (totalWeight > maximumWeight)
            {
                canCarry = false;
            }
            return canCarry;
        }

        public void EquipItem()
        {
            string returnString = "";
            hp += 101;
            returnString += hp;
        }

        /**
         * determines if the player got hit
         */
        public int GotHit()
        {
            if (currentRoom.getMonster(name).isAlive())
            {
                return hp -= 100;
            }
            return GotHit();
        }

        public string attack(string name)
        {
            string returnString = "";
            Monster monster = currentRoom.getMonster(name);
            if (monster == null)
            {
                returnString += "that monster is no monster in this room";
            }
            else if (GetInventoryString().contains("sword"))
            {
                hp -= 100;
                returnString += "\nyou attacked the monster" + "\nmonster HP: " + monster.getHp();
                returnString += "\nthe monster hit you back";
                monster.gotAttacked();
                returnString += "\n" + "your HP: " + hp;
            }
            else
            {
                returnString += " you dont have anything to attack" + name + "with";
            }

            return returnString;
        }

        /**
         * checks to see if there are any turns left. if there arent then it
         * is game over.
         */
        public bool gameOver()
        {
            return hp == 0;
        }

        /**
         * returns items displayed in the inventory
         */
        public string GetInventorystring()
        {
            int totalWeight = backpack.getTotalWeight();
            return backpack.inventoryTostring() + "\nweight: " + totalWeight + "/" + maximumWeight + "\nHP:" + hp;
        }

        /**
         * charges the beamer to memorize the current room
         */
        public string BeamerCharge()
        {
            string returnstring = "";
            chargeRoom = currentRoom;
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
                enterRoom(chargeRoom);
                returnstring += "fired beamer:" + "\n" + getRoomDescription();
            }
            else
            {
                returnstring += "you have to charge the beamer first";
            }
            return returnstring;
        }
    }
}
