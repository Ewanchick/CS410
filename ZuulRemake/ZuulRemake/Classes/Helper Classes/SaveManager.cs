using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Text.Json;
using ZuulRemake.Models;

namespace ZuulRemake.Classes.Helper_Classes
{
    public static class SaveManager
    {
        public class SaveData
        {
            public string? Name { get; set; }
            public int HP { get; set; }
            public int Level { get; set; }
            public int MaxWeight { get; set; }
        }

        public static SaveData ToSaveData(Player p)
        {
            return new SaveData
            {
                Name = p.Name,
                HP = p.HP,
                Level = p.Level,
                MaxWeight = p.MaxWeight
            };
        }

        private static string GetPath()
        {
            string basePath = AppContext.BaseDirectory;
            string folder = Path.Combine(basePath, "Saves");

            Directory.CreateDirectory(folder);
            return Path.Combine(folder, "savegame.json");
        }

        public static void Save(Player p) 
        {
            var data = ToSaveData(p);
            var options = new JsonSerializerOptions {  WriteIndented = true };
            string json = JsonSerializer.Serialize(p, options);
            File.WriteAllText(GetPath(), json);
        }

        public static SaveData? Load()
        {
            string path = GetPath();
            if (!File.Exists(path)) return null;

            string json = File.ReadAllText(path);            
            try
            {
                return JsonSerializer.Deserialize<SaveData>(json);
            }
            catch
            {
                Console.WriteLine("\nError retreiving save data.\n");                                     
                return null;
            }
        }
    }
}
