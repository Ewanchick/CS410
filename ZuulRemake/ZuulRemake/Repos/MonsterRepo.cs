using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ZuulRemake.Models;
using static ZuulRemake.Models.Model;

namespace ZuulRemake.Repos
{
    public class MonsterRepo
    {
        private readonly Model.GameContext _context;

        public MonsterRepo(Model.GameContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // CREATE
        public void Add(MonsterEntity monster)
        {
            if (monster == null) throw new ArgumentNullException(nameof(monster));
            _context.Monsters.Add(monster);
            _context.SaveChanges();
        }

        // READ (by id)
        public MonsterEntity? GetById(int id)
        {
            return _context.Monsters.FirstOrDefault(m => m.Id == id);
        }

        // READ (all)
        public List<MonsterEntity> GetAll()
        {
            return _context.Monsters.ToList();
        }

        // READ (by name)
        public MonsterEntity? GetByName(string name)
        {
            return _context.Monsters
                .FirstOrDefault(m => m.Name != null &&
                    m.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        // UPDATE
        public void Update(MonsterEntity monster)
        {
            if (monster == null) throw new ArgumentNullException(nameof(monster));
            _context.Monsters.Update(monster);
            _context.SaveChanges();
        }

        // DELETE
        public void Delete(int id)
        {
            var monster = _context.Monsters.Find(id);
            if (monster != null)
            {
                _context.Monsters.Remove(monster);
                _context.SaveChanges();
            }
        }
    }
}