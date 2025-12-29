using System.Text.Json;
using System.Text.Json.Serialization;
using TextBasedGame.Equipment.EquipmentStats.WeaponStats;

namespace TextBasedGame.Equipment.EquipmentStats.WeaponStats;

public class WeaponData
{
    [JsonPropertyName("displayName")]
    public string Name { get; set; }
    [JsonPropertyName("baseStats")]
    public Dictionary<WeaponStatType, float> weaponStats { get; set; }
}

public class WeaponLoader
{
    public static WeaponStats LoadStatsFromFile(string filePath)
    {
        // 1. Read the raw text from the file
        var jsonString = File.ReadAllText(filePath);

        // 2. Configure the loader to understand Enums (StatType)
        var options = new JsonSerializerOptions();
        
        options.PropertyNameCaseInsensitive = true;
        options.Converters.Add(new JsonStringEnumConverter());

        // 3. "Unpack" the JSON into our temporary blueprint
        var data = JsonSerializer.Deserialize<WeaponData>(jsonString, options);

        // 4. Create the actual Stats class using the dictionary we just unpacked
        return new WeaponStats(data.Name, data.weaponStats);
    }
}