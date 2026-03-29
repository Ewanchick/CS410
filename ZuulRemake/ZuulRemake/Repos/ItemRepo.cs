using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using ZuulRemake.Models;
using static ZuulRemake.Models.Model;

namespace ZuulRemake.Repos
{
    public class ItemRepo
    {
        private readonly Model.GameContext _context;

        public ItemRepo(Model.GameContext context)
        {
            _context = context;
        }

        // CREATE
        public void Add(ItemEntity item)
        {
            _context.Items.Add(item);
            _context.SaveChanges();
        }

        // READ (by id)
        public ItemEntity? GetById(int id)
        {
            return _context.Items.FirstOrDefault(i => i.Id == id);
        }

        // READ (all)
        public List<ItemEntity> GetAll()
        {
            return _context.Items.ToList();
        }

        // READ (by name)
        public ItemEntity? GetByName(string name)
        {
            return _context.Items
                .FirstOrDefault(i => i.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        // UPDATE
        public void Update(ItemEntity item)
        {
            _context.Items.Update(item);
            _context.SaveChanges();
        }

        // DELETE
        public void Delete(int id)
        {
            var item = _context.Items.Find(id);

            if (item != null)
            {
                _context.Items.Remove(item);
                _context.SaveChanges();
            }
        }
    }
}
