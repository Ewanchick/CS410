using System;
using System.Collections.Generic;
using System.Linq;
using ZuulRemake.Classes;
using ZuulRemake.Models;
using ZuulRemake.Repos;

namespace ZuulRemake.Services
{
    public class MonsterService
    {
        private readonly MonsterRepo monsterRepo;

        internal MonsterService(MonsterRepo monsterRepo)
        {
            this.monsterRepo = monsterRepo ?? throw new ArgumentNullException(nameof(monsterRepo));
        }

        // CREATE
        public void CreateMonster(string name, int hp, int level)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Monster name is required.", nameof(name));
            if (hp <= 0)
                throw new ArgumentException("HP must be greater than 0.", nameof(hp));
            if (level < 1)
                throw new ArgumentException("Level must be at least 1.", nameof(level));

            var monsterEntity = new Model.MonsterEntity
            {
                Name = name,
                HP = hp,
                Level = level
            };

            monsterRepo.Add(monsterEntity);
        }

        // READ
        public Monster? GetMonsterById(int id)
        {
            var entity = monsterRepo.GetById(id);
            return entity != null ? MapEntityToDomain(entity) : null;
        }

        public Monster? GetMonsterByName(string name)
        {
            var entity = monsterRepo.GetByName(name);
            return entity != null ? MapEntityToDomain(entity) : null;
        }

        public List<Monster> GetAllMonsters()
        {
            return monsterRepo.GetAll()
                .Select(MapEntityToDomain)
                .ToList();
        }

        // UPDATE
        public void UpdateMonster(int id, int? newHp = null, int? newLevel = null)
        {
            var monster = monsterRepo.GetById(id);
            if (monster == null)
                throw new InvalidOperationException($"Monster with ID {id} not found.");

            if (newHp.HasValue && newHp > 0)
                monster.HP = newHp.Value;

            if (newLevel.HasValue && newLevel >= 1)
                monster.Level = newLevel.Value;

            monsterRepo.Update(monster);
        }

        // DELETE
        public void DeleteMonster(int id)
        {
            var monster = monsterRepo.GetById(id);
            if (monster == null)
                throw new InvalidOperationException($"Monster with ID {id} not found.");

            monsterRepo.Delete(id);
        }

        // UTILITY
        public bool MonsterExists(string name)
        {
            return monsterRepo.GetByName(name) != null;
        }

        public int GetMonsterCount()
        {
            return monsterRepo.GetAll().Count;
        }

        public bool IsMonsterAlive(int id)
        {
            var monster = monsterRepo.GetById(id);
            return monster?.HP > 0;
        }

        private Monster MapEntityToDomain(Model.MonsterEntity entity)
        {
            return new Monster(
                entity.Name ?? "Unknown",
                entity.HP,
                entity.Level,
                null); // TODO: map Drop item if needed
        }
    }
}