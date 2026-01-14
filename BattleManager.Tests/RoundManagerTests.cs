using TextBasedGame;
using TextBasedGame.Characters;
using TextBasedGame.Characters.CharacterStats;
using TextBasedGame.DamageMechanics.Body;
using Xunit;

namespace BattleManager.Tests;

public class RoundManagerTests
{
    [Fact]
    public void AttackAgainstDeadEnemiesAreSkipped()
    {
        // Arrange
        var enemyStats = CharacterLoader.LoadStatsFromFile(GamePaths.CHARACTER_STATS + "Ogre.json");
        var enemy = new TestCharacter("Orc", baseStats:enemyStats);

        var heroStats = CharacterLoader.LoadStatsFromFile(GamePaths.CHARACTER_STATS + "TestDefaultHero.json");
        var hero = new TestCharacter("Halfdan", baseStats: heroStats);

        foreach (var part in enemy.Body.Parts.Values)
        {
            part.Effectiveness = 0;
        }

        var headPart = enemy.Body.GetPart(BodyPartType.Head);
        var initialEffectiveness = headPart.Effectiveness;
        
        // Act
        var roundOne = new RoundManager(hero, enemy);
        roundOne.Attack(hero, enemy);
        
        // Assert
        Assert.Equal(initialEffectiveness, headPart.Effectiveness);
    }
    
}