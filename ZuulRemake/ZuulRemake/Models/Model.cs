using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ZuulRemake.Classes;

namespace ZuulRemake.Models
{
    public class Model
    {
        public class GameContext : DbContext
        {
            public DbSet<PlayerEntity> Players { get; set; }
            public DbSet<MonsterEntity> Monsters { get; set; }
            public DbSet<RoomEntity> Rooms { get; set; }
            public DbSet<ExitEntity> Exits { get; set; }
            public DbSet<ItemEntity> Items { get; set; }
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
        public class PlayerEntity
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public int HP { get; set; }
            public int Level { get; protected set; }
            public bool IsAlive => HP > 0;

            protected List<Item> Inventory = [];
            public int CarryWeight => Inventory.Sum(i => i.Weight);
            public int MaxWeight { get; private set; } = 2;

            public int CurrentRoomId { get; private set; }
            private Room? ChargeRoom { get; set; }

        }
        public class MonsterEntity
        {
            public int Id { get; set; }
            public required string Name { get; set; }
            public int HP { get; set; }
            public int Level { get; set; }
            public bool IsAlive => HP > 0;
            public Item? Drop { get; set; }
        }
        public class RoomEntity
        {
            public int Id { get; set; }
            public required string Name { get; set; }
            public required string NarrativeDescription { get; set; }
            public required string LongDescription { get; set; }

            public List<ItemEntity>? Items;
            public List<MonsterEntity>? Monsters;
            public List<ExitEntity>? Exits;
        }
        public class ExitEntity
        {
            public int Id { get; set; }
            public string? Direction { get; set; }
            public Room? TargetRoom { get; set; }
            public bool IsLocked { get; set; }
            public int RoomId { get; set; }
        }

        public class ItemEntity
        {
            public int Id { get; set; }
            public required string Description { get; set; }
            public int Weight { get; set; }
            public required string Name { get; set; }
            public int? StatIncrease { get; set; }
        }
        public class RoomItem
        {
            public int Id { get; set; }
            public Room? Room { get; set; }
            public List<Item>? Items { get; set; }
        }
        public class RoomMonster
        {
            public int Id { get; set; }
            public Room? Room { get; set; }
            public List<Monster>? Monsters { get; set; }
        }
        public class RoomExit
        {
            public int Id { get; set; }
            public required Room Room { get; set; }
            public List<Exit>? Exits { get; set; }
        }

    }
}
