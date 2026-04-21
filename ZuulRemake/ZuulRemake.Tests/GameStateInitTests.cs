using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZuulRemake.Classes;
using ZuulRemake.Web.Models;

namespace ZuulRemake.Tests
{
    public class GameStateInitTests
    {
        Player p = new Player("Name", 100, 10);

        public GameStateInitTests()
        {            
            Room entryway = new Room("Entryway", "This is the entryway.", " ");
            Item book = new Item("book", "old book", 1, null);
            Monster monster = new Monster("wmjs", 100, 10, null);
            Exit exit = new Exit("north", entryway, false);
            entryway.AddMonster(monster);
            entryway.AddItem(book);
            entryway.AddExit(exit);
            p.GoNewRoom(entryway);                       
        }

        [Fact]
        public void GameStateObjectsNotNull()
        {
            GameState gs = new GameState(p);

            Assert.NotNull(gs.player);
            Assert.NotNull(gs.player.CurrentRoom.GetMonstersOb());
            Assert.NotNull(gs.player.CurrentRoom.GetItemsOb());
            Assert.NotNull(gs.player.CurrentRoom.GetExitsOb());
        }

        [Fact]
        public void GameStateUpdates()
        {
            GameState gs = new GameState(p);

        }
    }
}
