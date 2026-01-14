using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using TextBasedGame.Characters.BaseStats;

namespace TextBasedGame.Characters.CharacterStats;

public class CharacterData
{
    public required string CharacterType { get; set; }
    public required string DisplayName { get; set; }
    public required Dictionary<CharacterStatType, float> BaseStats { get; set; }
}

public class CharacterLoader
{
    public static BaseStats.BaseStats LoadStatsFromFile(string filePath)
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
        return new BaseStats.BaseStats(data!.BaseStats);
    }
}

