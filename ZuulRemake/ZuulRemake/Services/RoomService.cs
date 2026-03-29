using System;
using System.Collections.Generic;
using System.Linq;
using ZuulRemake.Classes;
using ZuulRemake.Models;
using ZuulRemake.Repos;

namespace ZuulRemake.Services
{
    public class RoomService
    {
        private readonly RoomRepo roomRepo;

        public RoomService(RoomRepo roomRepo)
        {
            this.roomRepo = roomRepo ?? throw new ArgumentNullException(nameof(roomRepo));
        }

        // CREATE
        public void CreateRoom(string name, string narrativeDescription, string longDescription)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Room name is required.", nameof(name));

            var roomEntity = new Model.RoomEntity
            {
                Name = name,
                NarrativeDescription = narrativeDescription ?? "",
                LongDescription = longDescription ?? "",
                Items = new List<Model.ItemEntity>(),
                Monsters = new List<Model.MonsterEntity>(),
                Exits = new List<Model.ExitEntity>()
            };

            roomRepo.AddRoom(roomEntity);
            RoomMapper.RegisterRoomId(name, roomEntity.Id);
        }

        // READ
        public Room? GetRoomById(int id)
        {
            var entity = roomRepo.GetById(id);
            return entity != null ? MapEntityToDomain(entity) : null;
        }

        public Room? GetRoomByName(string name)
        {
            var entity = roomRepo.GetByName(name);
            return entity != null ? MapEntityToDomain(entity) : null;
        }

        public List<Room> GetAllRooms()
        {
            return roomRepo.GetAll()
                .Select(MapEntityToDomain)
                .ToList();
        }

        // UPDATE
        public void UpdateRoom(int id, string? narrativeDescription)
        {
            var room = roomRepo.GetById(id);
            if (room == null)
                throw new InvalidOperationException($"Room with ID {id} not found.");

            if (!string.IsNullOrWhiteSpace(narrativeDescription))
                room.NarrativeDescription = narrativeDescription;

            roomRepo.Update(room);
        }

        // DELETE
        public void DeleteRoom(int id)
        {
            var room = roomRepo.GetById(id);
            if (room == null)
                throw new InvalidOperationException($"Room with ID {id} not found.");

            roomRepo.Delete(id);
        }

        // UTILITY
        public bool RoomExists(string name)
        {
            return roomRepo.GetByName(name) != null;
        }

        public int GetRoomCount()
        {
            return roomRepo.GetAll().Count;
        }

        private Room MapEntityToDomain(Model.RoomEntity entity)
        {
            return new Room(
                entity.Name ?? "Unknown",
                entity.NarrativeDescription ?? "",
                entity.LongDescription ?? "");
        }
    }
}