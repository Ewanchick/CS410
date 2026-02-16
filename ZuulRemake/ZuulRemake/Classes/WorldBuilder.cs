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
            entryway = new Room("in the entryway of the castle, the exit to the south is locked");
            dininghall = new Room("in a large dining hall, you see a lantern on the floor and a ballroom to your right");
            ballroom = new Room("in the ballroom, you find armour in the middle of the floor");
            kitchen = new Room("in the kitchen, it is too dark to see anything");
            bathroom = new Room("in the bathroom you see a potion, but there is a nasty ghoul guarding it.");
            dungeon = new Room("in the dungeon, there is a large dragon guarding the key to the exit");
            bedroom = new Room("in a very large bedroom, nothing interesting in here.");
            exit = new Room("You made it, you escaped the castle and are now free!");

            // initialise room exits
            entryway.SetExit("north", dininghall);
            entryway.SetExit("east", kitchen);
            entryway.SetExit("west", bedroom);
            entryway.SetExit("down", dungeon);

            dininghall.SetExit("east", ballroom);
            dininghall.SetExit("south", entryway);
            ballroom.SetExit("west", dininghall);
            kitchen.SetExit("west", entryway);
            dungeon.SetExit("up", entryway);
            bedroom.SetExit("east", entryway);
            bedroom.SetExit("south", bathroom);
            bathroom.SetExit("north", bedroom);

            // create the items
            //var sword = new Item("sword", "heavy sword, might be used to kill the dragon", 1);
            var lantern = new Item("lantern", "used to light the dark rooms of the castle", 1);
            var armour = new Item("armour", "protect yourself from the mighty dragon", 1);
            var potion = new Item("potion", "use this to increase your health!", 1);
            var key = new Item("key", "used to unlock the way out", 0);

            // initialize items
            dininghall.SetItem("lantern", lantern);
            ballroom.SetItem("armour", armour);

            // create the monsters
            var dragon = new Monster("dragon", 100, 10, key);
            var ghoul = new Monster("ghoul", 50, 100, potion);

            dungeon.SetMonster("dragon", dragon);
            bathroom.SetMonster("ghoul", ghoul);

            // start room
            return entryway;
        }
    }
}
