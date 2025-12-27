using System.Text.Json;
using System.Text.Json.Serialization;
using TextBasedGame.Characters.Stats;
namespace TextBasedGame.Characters.CharacterData;

public class CharacterData
{
    public string CharacterType { get; set; }
    public string DisplayName { get; set; }
    public Dictionary<CharacterStatType, float> baseStats { get; set; }
}

public class CharacterLoader
{
    public static CharacterStats.CharacterStats LoadStatsFromFile(string filePath)
    {
        // 1. Read the raw text from the file
        var jsonString = File.ReadAllText(filePath);

        // 2. Configure the loader to understand Enums (StatType)
        var options = new JsonSerializerOptions();
        
        options.PropertyNameCaseInsensitive = true;
        options.Converters.Add(new JsonStringEnumConverter());

        // 3. "Unpack" the JSON into our temporary blueprint
        var data = JsonSerializer.Deserialize<CharacterData>(jsonString, options);

        // 4. Create the actual Stats class using the dictionary we just unpacked
        return new CharacterStats.CharacterStats(data.baseStats);
    }
}

