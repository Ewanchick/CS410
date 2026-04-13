using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using ZuulRemake.Models;
using static ZuulRemake.Models.Model;

namespace ZuulRemake.Repos
{
    public class ItemRepo : RepoBase<Model.ItemEntity>, IItemRepo
    {
        

        public ItemRepo(Model.GameContext context) : base(context)
        {
        }

        

        // READ (by name)
        public ItemEntity? GetByName(string name)
        {
            return _context.Items
                .FirstOrDefault(i => i.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        }
    public interface IItemRepo : IRepository<Model.ItemEntity>
    {
        Model.ItemEntity? GetByName(string name);
    }
}

