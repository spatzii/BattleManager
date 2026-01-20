using System;
using TextBasedGame;
using TextBasedGame.Characters;
using TextBasedGame.Characters.CharacterStats;
using TextBasedGame.Combat;
using TextBasedGame.DamageMechanics.Body;
using TextBasedGame.Equipment.EquipmentStats.WeaponStats;
using TextBasedGame.Equipment.Weapons;
using Xunit;

namespace BattleManager.Tests;

public class UnitTest1
{
    [Fact]
    public void ResolveAttack_TwiceAgainstSameDefender_PersistsDamageAcrossCalls()
    {
        // Arrange
        var sword = Sword.Create();
        
        var enemyStats = CharacterLoader.LoadStatsFromFile(GamePaths.CHARACTER_STATS + "Ogre.json");
        var enemy = new TestCharacter("Orc", baseStats:enemyStats, sword);

        var heroStats = CharacterLoader.LoadStatsFromFile(GamePaths.CHARACTER_STATS + "TestDefaultHero.json");
        var hero = new TestCharacter("Halfdan", baseStats:heroStats, sword);
        
        var resolver = new Combat(hero, enemy);

        var startEffectiveness = enemy.Body.GetPart(BodyPartType.Head).Effectiveness; // currently 100 in Head

        // Act
        var afterFirstAttack = resolver.TestResolveAttackHead();
        var afterSecondAttack = resolver.TestResolveAttackHead();

        // Assert (persistence: the second call builds on the state changed by the first)
        Assert.Equal(startEffectiveness - hero.Weapon!.Stats.Damage, afterFirstAttack);
        Assert.Equal(startEffectiveness - (2 * hero.Weapon!.Stats.Damage), afterSecondAttack);

        // Also assert the defender's state actually changed (not just the return value)
        Assert.Equal(afterSecondAttack, enemy.Body.GetPart(BodyPartType.Head).Effectiveness);
    }
    
    [Fact]
    public void ResolveAttack_RandomAttack_TwoHitsPersist()
    {
        // Arrange
        
        var sword = Sword.Create();
        
        var enemyStats = CharacterLoader.LoadStatsFromFile(GamePaths.CHARACTER_STATS + "Ogre.json");
        var enemy = new TestCharacter("Orc", baseStats:enemyStats, sword);

        var heroStats = CharacterLoader.LoadStatsFromFile(GamePaths.CHARACTER_STATS + "TestDefaultHero.json");
        var hero = new TestCharacter("Halfdan", baseStats:heroStats, sword);
        
        var resolver = new Combat(hero, enemy);

        var randPart = enemy.Body.GetRandomPart();

        var startEffectiveness = randPart.Effectiveness; // currently 100 in random

        // Act
        var afterFirstAttack = resolver.ResolveAttack_SpecificPart(randPart);
        var afterSecondAttack = resolver.ResolveAttack_SpecificPart(randPart);

        // Assert (persistence: the second call builds on the state changed by the first)
        Assert.Equal(startEffectiveness - hero.Weapon!.Stats.Damage, afterFirstAttack);
        Assert.Equal(startEffectiveness - (2 * hero.Weapon!.Stats.Damage), afterSecondAttack);

        // Also assert the defender's state actually changed (not just the return value)
        Assert.Equal(afterSecondAttack, randPart.Effectiveness);
    }

    [Fact]
    public void ResolveAttack_Sword_vs_Club_TestAttackHead()
    {
        // Arrange

        var sword = Sword.Create();
        var club = Club.Create();
        
        var enemyStats = CharacterLoader.LoadStatsFromFile(GamePaths.CHARACTER_STATS + "Ogre.json");
        var enemy = new TestCharacter("Orc", baseStats:enemyStats, club);

        var heroStats = CharacterLoader.LoadStatsFromFile(GamePaths.CHARACTER_STATS + "TestDefaultHero.json");
        var hero = new TestCharacter("Halfdan", baseStats:heroStats, sword);
        
        var resolverHero = new Combat(hero, enemy);
        var resolverEnemy = new Combat(enemy, hero);
        var heroStartEffectiveness = hero.Body.GetPart(BodyPartType.Head).Effectiveness; // currently 100 in Head
        var enemyStartEffectiveness = enemy.Body.GetPart(BodyPartType.Head).Effectiveness; // currently 100 in Head
        
        // Act

        var enemyAfterFirstAttack = resolverHero.TestResolveAttackHead();
        var heroAfterFirstAttack = resolverEnemy.TestResolveAttackHead();
        
        // Assert
        Assert.True(heroAfterFirstAttack > enemyAfterFirstAttack);
        Assert.Equal(92f, heroAfterFirstAttack);
        Assert.Equal(90f, enemyAfterFirstAttack);
    }

    [Fact]
    public void CheckAverageEffectivenessOfLimbs()
    {
        var sword = Sword.Create();
        var club = Club.Create();
        var enemyStats = CharacterLoader.LoadStatsFromFile(GamePaths.CHARACTER_STATS + "Ogre.json");
        var enemy = new TestCharacter("Orc", baseStats:enemyStats, club);
        var heroStats = CharacterLoader.LoadStatsFromFile(GamePaths.CHARACTER_STATS + "TestDefaultHero.json");
        var hero = new TestCharacter("Halfdan", baseStats:heroStats, sword);

        var resolverHero = new Combat(hero, enemy);
        
        // Act
        resolverHero.TestResolveAttackHead();
        var condition = MathF.Round(enemy.GetOverallCondition(), 1);

        // assert
        
        Assert.Equal(98.3f, condition);

    }
}