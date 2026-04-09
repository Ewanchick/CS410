using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZuulRemake.Models;

namespace ZuulRemake.Repos
{
    public abstract class RepoBase<T> : IRepository<T> where T : class
    {
        protected readonly Model.GameContext _context;
        protected readonly DbSet<T> table;

        protected RepoBase(Model.GameContext context)
        {
            _context = context;
            this.table = context.Set<T>();
        }

        public virtual T? GetById(int id) => table.Find(id);
        public virtual List<T> GetAll() => table.ToList();
        public virtual void Add(T entity)
        {
            table.Add(entity);
            _context.SaveChanges();
        }

        public virtual void AddRange(IEnumerable<T> entities)
        {
            table.AddRange(entities);
            _context.SaveChanges();
        }

       

        public virtual void Update(T entity)
        {
            table.Update(entity);
            _context.SaveChanges();
        }

        public virtual void Delete(int id)
        {
            var entity = table.Find(id);
            if (entity != null)
            {
                table.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}
