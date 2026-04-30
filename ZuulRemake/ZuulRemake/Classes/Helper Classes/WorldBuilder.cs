using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZuulRemake.Classes
{
    public class WorldBuilder
    {
        public static Room Build(
                out Room entryway,
                out Room dininghall,
                out Room ballroom,
                out Room kitchen,
                out Room bathroom,
                out Room dungeon,
                out Room bedroom,
                out Room freedom)
        {            
            // ROOMS            
            entryway = new Room("Entryway", "You're surrounded by tall, old stone walls. All around you are doors to other rooms. Who knows what lies ahead...", " ");
            dininghall = new Room("Dining Hall", "A lone table sits empty, some chairs overturned... A layer of dust has settled on the dishes.", " ");
            ballroom = new Room("Ballroom", "Another massive, empty room. The floor was once beautiful, but now is cracked and unpolished.", " ");
            kitchen = new Room("Kitchen", "The smell of charred wood hangs in the air, and it is very dark. A lantern would be useful here...", " ");
            bathroom = new Room("Bathroom", "There's a terrible smell in here. Surely, evil lurks nearby...", " ");
            dungeon = new Room("Dungeon", "This must be the lair of the beast!", " ");
            bedroom = new Room("Bedroom", "Musty, empty, and coated with a layer of dust.", " ");
            freedom = new Room("Exit", "The door has been opened. Freedom awaits!", " ");
            
            // ENTRYWAY EXITS            
            entryway.AddExit(new Exit("north", dininghall, false));
            entryway.AddExit(new Exit("south", freedom, true)); // locked
            entryway.AddExit(new Exit("east", ballroom, false));
            entryway.AddExit(new Exit("west", bedroom, false));
            entryway.AddExit(new Exit("down", dungeon, false));
            
            // DINING HALL EXITS            
            dininghall.AddExit(new Exit("south", entryway, false));
            dininghall.AddExit(new Exit("east", kitchen, false));
            
            // KITCHEN EXITS            
            kitchen.AddExit(new Exit("west", dininghall, false));
            kitchen.AddExit(new Exit("south", ballroom, false));
            
            // BALLROOM EXITS            
            ballroom.AddExit(new Exit("north", kitchen, false));
            ballroom.AddExit(new Exit("west", entryway, false));
            
            // BEDROOM EXITS            
            bedroom.AddExit(new Exit("east", entryway, false));
            bedroom.AddExit(new Exit("south", bathroom, false));
            
            // BATHROOM EXITS            
            bathroom.AddExit(new Exit("north", bedroom, false));
            
            // DUNGEON EXITS            
            dungeon.AddExit(new Exit("up", entryway, false));

            // create the items
            var sword = new Item("Sword", "Heavy and sharp, capable of slaying the mightiest beast.", 1, 50);
            var lantern = new Item("Lantern", "This should be able to light up any dark rooms.", 1, 0);
            var armour = new Item("Armour", "Protect yourself from the lurking dangers!", 1, 20);
            var potion = new Item("Potion", "Use this to increase your health!", 1, 50);
            var key = new Item("Key", "This looks like it should fit the lock in the Entryway...", 0, 0);

            // initialize items
            entryway.AddItem(sword);
            dininghall.AddItem(lantern);
            ballroom.AddItem(armour);

            // create the monsters
            var dragon = new Monster("Dragon", 100, 50, key);
            var ghoul = new Monster("Ghoul", 100, 30, potion);

            dungeon.AddMonster(dragon);
            bathroom.AddMonster(ghoul);

            // start room
            return entryway;
        }
    }
}