using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using ZuulRemake.Models;
using ZuulRemake.Classes;
using static ZuulRemake.Models.Model;

namespace ZuulRemake.Repos
{
    public class RoomRepo : RepoBase<Model.RoomEntity>, IRoomRepo
    {
        
        public RoomRepo(Model.GameContext context) : base(context)
        
            {
              
            }
        
        

        public RoomEntity? GetByName(string name)
        {
            return _context.Rooms
                .Include(r => r.Items)
                .Include(r => r.Monsters)
                .Include(r => r.Exits)
                .FirstOrDefault(r => r.Name == name);
        }

        }

    public interface IRoomRepo : IRepository<Model.RoomEntity>
    {
        Model.RoomEntity? GetByName(string name);
    }

}

