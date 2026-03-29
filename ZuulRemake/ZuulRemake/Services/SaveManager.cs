using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using ZuulRemake.Classes;

namespace ZuulRemake.Services
{
    public class SaveManager
    {
        private readonly string SavesDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ZuulRemake", "Saves");

        public SaveManager()
        {
            Directory.CreateDirectory(SavesDirectory);
        }

        /// <summary>
        /// Save player stats to JSON file. 
        /// Inventory and current room are saved separately to the database via PlayerEntity.
        /// </summary>
        public void SavePlayerStatsToJson(Player player, string fileName = "player_save.json")
        {
            var saveData = new SaveData
            {
                Name = player.Name,
                HP = player.HP,
                Level = player.Level
            };

            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(saveData, options);
            var filePath = Path.Combine(SavesDirectory, fileName);

            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Load player stats from JSON file.
        /// </summary>
        public SaveData? LoadPlayerStatsFromJson(string fileName = "player_save.json")
        {
            var filePath = Path.Combine(SavesDirectory, fileName);

            if (!File.Exists(filePath))
                return null;

            var json = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            return JsonSerializer.Deserialize<SaveData>(json, options);
        }
    }
}

public class SaveData
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("hp")]
    public int HP { get; set; }

    [JsonPropertyName("level")]
    public int Level { get; set; }
}
