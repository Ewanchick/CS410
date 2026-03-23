using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZuulRemake.Classes;

namespace ZuulRemake.Models
{
    internal class Model
    {
        public class GameContext : DbContext
        {
            public DbSet<Player> Players { get; set; }
            public DbSet<Monster> Monsters { get; set; }
            public DbSet<Room> Rooms { get; set; }
            public DbSet<Exit> Exits { get; set; }
            public DbSet<Item> Items { get; set; }
            public string DbPath { get; }
            public GameContext()
            {
                var folder = Environment.SpecialFolder.LocalApplicationData;
                var path = Environment.GetFolderPath(folder);
                DbPath = System.IO.Path.Join(path, "Game.db");
            }          
            //Creates the database if one is not present
                protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
        }
    }
}
