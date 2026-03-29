using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using ZuulRemake.Models;
using ZuulRemake.Classes;
using static ZuulRemake.Models.Model;

namespace ZuulRemake.Repos
{
    public class RoomRepo
    {
        private readonly Model.GameContext _context;
        public RoomRepo(Model.GameContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

        }
        public void AddRoom(Model.RoomEntity room)
        {
            if (room == null) throw new ArgumentNullException(nameof(room));

            _context.Rooms.Add(room);
            _context.SaveChanges();
            
        }
        public void AddRooms(IEnumerable<Model.RoomEntity> rooms)
        {
            if (rooms == null) throw new ArgumentNullException(nameof(rooms));
            _context.Rooms.AddRange(rooms);
            _context.SaveChanges();

            // After saving, register all room IDs so exits can reference them
            foreach (var room in rooms)
            {
                if (room.Id > 0 && room.Name != null)
                {
                    RoomMapper.RegisterRoomId(room.Name, room.Id);
                }
            }
        }


        public RoomEntity? GetById(int id)
        {
            return _context.Rooms
                .Include(r => r.Items)
                .Include(r => r.Monsters)
                .Include(r => r.Exits)
                .FirstOrDefault(r => r.Id == id);
        }

        public List<RoomEntity> GetAll()
        {
            return _context.Rooms
                .Include(r => r.Items)
                .Include(r => r.Monsters)
                .Include(r => r.Exits)
                .ToList();
        }

        public RoomEntity? GetByName(string name)
        {
            return _context.Rooms
                .Include(r => r.Items)
                .Include(r => r.Monsters)
                .Include(r => r.Exits)
                .FirstOrDefault(r => r.Name == name);
        }

        public void Update(RoomEntity room)
        {
            _context.Rooms.Update(room);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var room = _context.Rooms.Find(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                _context.SaveChanges();
            }
        }
    }
}
