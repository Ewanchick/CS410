using System;
using System.Collections.Generic;
using System.Linq;
using ZuulRemake.Classes;
using ZuulRemake.Models;
using ZuulRemake.Repos;

namespace ZuulRemake.Services
{
    public class ItemService
    {
        private readonly ItemRepo itemRepo;

        public ItemService(ItemRepo itemRepo)
        {
            this.itemRepo = itemRepo ?? throw new ArgumentNullException(nameof(itemRepo));
        }

        // CREATE
        public void CreateItem(string name, string description, int weight)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Item name is required.", nameof(name));
            if (weight < 0)
                throw new ArgumentException("Weight cannot be negative.", nameof(weight));

            var itemEntity = new Model.ItemEntity
            {
                Name = name,
                Description = description ?? "",
                Weight = weight
            };

            itemRepo.Add(itemEntity);
        }

        // READ
        public Item? GetItemById(int id)
        {
            var entity = itemRepo.GetById(id);
            return entity != null ? MapEntityToDomain(entity) : null;
        }

        public Item? GetItemByName(string name)
        {
            var entity = itemRepo.GetByName(name);
            return entity != null ? MapEntityToDomain(entity) : null;
        }

        public List<Item> GetAllItems()
        {
            return itemRepo.GetAll()
                .Select(MapEntityToDomain)
                .ToList();
        }

        // UPDATE
        public void UpdateItem(int id, string? newDescription = null, int? newWeight = null)
        {
            var item = itemRepo.GetById(id);
            if (item == null)
                throw new InvalidOperationException($"Item with ID {id} not found.");

            if (!string.IsNullOrWhiteSpace(newDescription))
                item.Description = newDescription;

            if (newWeight.HasValue && newWeight >= 0)
                item.Weight = newWeight.Value;

            itemRepo.Update(item);
        }

        // DELETE
        public void DeleteItem(int id)
        {
            var item = itemRepo.GetById(id);
            if (item == null)
                throw new InvalidOperationException($"Item with ID {id} not found.");

            itemRepo.Delete(id);
        }

        // UTILITY
        public bool ItemExists(string name)
        {
            return itemRepo.GetByName(name) != null;
        }

        public int GetItemCount()
        {
            return itemRepo.GetAll().Count;
        }

        public List<Item> GetItemsByWeight(int maxWeight)
        {
            return itemRepo.GetAll()
                .Where(i => i.Weight <= maxWeight)
                .Select(MapEntityToDomain)
                .ToList();
        }

        private Item MapEntityToDomain(Model.ItemEntity entity)
        {
            return new Item(
                entity.Name ?? "Unknown",
                entity.Description ?? "",
                entity.Weight,
                entity.StatIncrease
                );
        }
    }
}