using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Reflection.Metadata;

namespace ZuulRemake.Classes.Helper_Classes
{
    public class ObjectSaver
    {
        public ObjectSaver()
        {
            string directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "JSON");
            string filePath = Path.Combine(directory, "player.json");
            Directory.CreateDirectory(directory);
        }

        public void Save(string filePath, Player p)
        {
            string jsonString = JsonSerializer.Serialize(p);
            File.WriteAllText(filePath, jsonString);
            Console.WriteLine($"Saved to: {filePath}");
        }
    }
}
