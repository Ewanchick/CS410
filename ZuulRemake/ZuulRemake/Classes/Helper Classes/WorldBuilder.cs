using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZuulRemake.Classes
{
    internal class WorldBuilder
    {
        public static Room Build(
                out Room entryway,
                out Room dininghall,
                out Room ballroom,
                out Room kitchen,
                out Room bathroom,
                out Room dungeon,
                out Room bedroom,
                out Room exit)
        {
            // create the rooms
            entryway = new Room("Entryway", "You enter into the entry way of the castle dim lights", " ");
            dininghall = new Room("Dining Hall", "In a large dining hall, you see a" ," ");
            ballroom = new Room("Ballroom", "In the ballroom, you find armour in the middle of the floor", " ");
            kitchen = new Room("Kicthen","You step into the damp, musty kitchen. It is much too dark to see any", " ");
            bathroom = new Room("Bathroom","As you enter the bathroom you a pair of bloodshot eyes glaring at you from the darkness", " ");
            dungeon = new Room("Dungeon", "In the dungeon, there is a large dragon guarding the key to the exit", " ");
            bedroom = new Room("Bedroom", "In a very large bedroom, nothing interesting in here."," ");
            exit = new Room("Exit","You made it, you escaped the castle and are now free!"," ");

            // initialise room exits
            entryway.AddExit("north", dininghall, false);
            entryway.AddExit("east", kitchen, false);
            entryway.AddExit("west", bedroom, false);
            entryway.AddExit("south", dungeon, true);

            dininghall.AddExit("east", ballroom, false);
            dininghall.AddExit("south", entryway, false);
            ballroom.AddExit("west", dininghall, false);

            kitchen.AddExit("west", entryway, false);

            dungeon.AddExit("up", entryway, false);
            bedroom.AddExit("east", entryway, false);
            bedroom.AddExit("south", bathroom, false);
            bathroom.AddExit("north", bedroom, false);

            // create the items
            var sword = new Item("sword", "heavy sword, might be used to kill the dragon", 1, 10);
            var lantern = new Item("lantern", "used to light the dark rooms of the castle", 1, 0);
            var armour = new Item("armour", "protect yourself from the mighty dragon", 1, 20);
            var potion = new Item("Potion", "use this to increase your health!", 1, 50);
            var key = new Item("key", "used to unlock the way out", 0, 0);

            // initialize items
            dininghall.AddItem(lantern);
            ballroom.AddItem(armour);

            // create the monsters
            var dragon = new Monster("dragon", 100, 10, key);
            var ghoul = new Monster("ghoul", 50, 100, potion);

            dungeon.AddMonster(dragon);
            bathroom.AddMonster(ghoul);
            //update long description 

            // start room
            return entryway;
        }
    }
}