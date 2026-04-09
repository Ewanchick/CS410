using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ZuulRemake.Models;
using static ZuulRemake.Models.Model;

namespace ZuulRemake.Repos
{
    public class MonsterRepo :  RepoBase<MonsterEntity>
    {
       

        public MonsterRepo(Model.GameContext context) : base(context)
        {
        }

        // READ (by name)
        public virtual MonsterEntity? GetByName(string name)
        {
            return _context.Monsters
                .FirstOrDefault(m => m.Name != null &&
                    m.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        }

    public interface IMonsterRepo : IRepository<Model.MonsterEntity>
    {
        Model.MonsterEntity? GetByName(string name);
    }

}
