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

            Exit ex0 = new Exit("north", dininghall, false);
            Exit ex1 = new Exit("east", kitchen, false);
            Exit ex2 = new Exit("west", bedroom, false);
            Exit ex3 = new Exit("down", dungeon, false);
            Exit ex4 = new Exit("south", exit, true);

            Exit ex5 = new Exit("east", ballroom, false);
            Exit ex6 = new Exit("south", entryway, false);
            Exit ex8 = new Exit("west", dininghall, false);

            Exit ex9 = new Exit("west", entryway, false);

            Exit ex10 = new Exit("up", entryway, false);
            Exit ex11 = new Exit("east", entryway, false);
            Exit ex12 = new Exit("south", bathroom, false);
            Exit ex13 = new Exit("north", bedroom, false);

            entryway.AddExit(ex0);
            entryway.AddExit(ex1);
            entryway.AddExit(ex2);
            entryway.AddExit(ex3);
            entryway.AddExit(ex4);
            dininghall.AddExit(ex5);
            dininghall.AddExit(ex6);
            ballroom.AddExit(ex8);
            kitchen.AddExit(ex9);
            dungeon.AddExit(ex10);
            bedroom.AddExit(ex11);
            bedroom.AddExit(ex12);
            bathroom.AddExit(ex13);

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
            var dragon = new Monster("Dragon", 100, 50, key);
            var ghoul = new Monster("Ghoul", 100, 30, potion);

            dungeon.AddMonster(dragon);
            bathroom.AddMonster(ghoul);
            //update long description 

            // start room
            return entryway;
        }
    }
}