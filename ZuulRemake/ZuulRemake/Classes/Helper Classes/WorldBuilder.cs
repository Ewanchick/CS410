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
            entryway = new Room("Entryway", "You're surrounded by tall, old stone walls, and a chandelier flickers overhead.", " ");
            dininghall = new Room("Dining Hall", "A long table sits empty, some chairs overturned... A layer of dust has settled on the dishes." ," ");
            ballroom = new Room("Ballroom", "Another massive, empty room. The floor was once beautiful, but now is cracked and unpolished.", " ");
            kitchen = new Room("Kicthen","It is very dark and damp; it is far too dark to see without some torch, lantern, or candle...", " ");
            bathroom = new Room("Bathroom", "Something is lurking here... a pair of eyes stares out at you from the darkness!", " ");
            dungeon = new Room("Dungeon", "A massive beast rises to greet you, sharp teeth catching the light of the few torches lining the walls.", " ");
            bedroom = new Room("Bedroom", "Musty, empty, and coated with a layer of dust."," ");
            exit = new Room("Exit", "The door swings open, the light of the sunrise creeping in. You've made it out!"," ");

            // initialise room exits
            entryway.AddExit("north", dininghall, false);
            entryway.AddExit("east", kitchen, false);
            entryway.AddExit("west", bedroom, false);
            entryway.AddExit("down", dungeon, false);
            entryway.AddExit("south", exit, true);

            dininghall.AddExit("east", ballroom, false);
            dininghall.AddExit("south", entryway, false);
            ballroom.AddExit("west", dininghall, false);

            kitchen.AddExit("west", entryway, false);

            dungeon.AddExit("up", entryway, false);
            bedroom.AddExit("east", entryway, false);
            bedroom.AddExit("south", bathroom, false);
            bathroom.AddExit("north", bedroom, false);

            // create the items
            var sword = new Item("Sword", "Heavy and sharp, capable of slaying the mightiest beast.", 1, 50);
            var lantern = new Item("Lantern", "This should be able to light up any dark rooms.", 1, 0);
            var armour = new Item("Armour", "Protect yourself from the lurking dangers!", 1, 20);
            var potion = new Item("Potion", "Use this to increase your health!", 1, 50);
            var key = new Item("Key", "This looks like it should fit the lock in the Entryway...", 0, 0);

            // initialize items
            dininghall.AddItem(lantern);
            ballroom.AddItem(armour);


            // create the monsters
            var dragon = new Monster("dragon", 100, 50, key);
            var ghoul = new Monster("ghoul", 100, 30, potion);

            dungeon.AddMonster(dragon);
            bathroom.AddMonster(ghoul);
            //update long description 

            // start room
            return entryway;
        }
    }
}