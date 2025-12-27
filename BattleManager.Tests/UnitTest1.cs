using TextBasedGame;
using TextBasedGame.Characters;
using TextBasedGame.Characters.CharacterData;
using TextBasedGame.Characters.Stats;
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
        var swordStats = WeaponLoader.LoadStatsFromFile(GamePaths.WEAPON_STATS + "BasicSword.json");
        var sword = new Sword(weaponStats: swordStats);
        
        var enemyStats = CharacterLoader.LoadStatsFromFile(GamePaths.CHARACTER_STATS + "Ogre.json");
        var enemy = new GenericTestCharacter("Orc", characterStats:enemyStats, sword);

        var heroStats = CharacterLoader.LoadStatsFromFile(GamePaths.CHARACTER_STATS + "TestDefaultHero.json");
        var hero = new GenericTestCharacter("Halfdan", characterStats:heroStats, sword);
        
        var resolver = new CombatResolver();

        var startEffectiveness = enemy.Body.GetPart(BodyPartType.Head).Effectiveness; // currently 100 in Head

        // Act
        var afterFirstAttack = resolver.Test_ResolveAttack_Head(hero, enemy);
        var afterSecondAttack = resolver.Test_ResolveAttack_Head(hero, enemy);

        // Assert (persistence: the second call builds on the state changed by the first)
        Assert.Equal(startEffectiveness - hero.Weapon!.Damage, afterFirstAttack);
        Assert.Equal(startEffectiveness - (2 * hero.Weapon!.Damage), afterSecondAttack);

        // Also assert the defender's state actually changed (not just the return value)
        Assert.Equal(afterSecondAttack, enemy.Body.GetPart(BodyPartType.Head).Effectiveness);
    }
    
    [Fact]
    public void ResolveAttack_RandomAttack_TwoHitsPersist()
    {
        // Arrange
        
        var swordStats = WeaponLoader.LoadStatsFromFile(GamePaths.WEAPON_STATS + "BasicSword.json");
        var sword = new Sword(weaponStats: swordStats);
        
        var enemyStats = CharacterLoader.LoadStatsFromFile(GamePaths.CHARACTER_STATS + "Ogre.json");
        var enemy = new GenericTestCharacter("Orc", characterStats:enemyStats, sword);

        var heroStats = CharacterLoader.LoadStatsFromFile(GamePaths.CHARACTER_STATS + "TestDefaultHero.json");
        var hero = new GenericTestCharacter("Halfdan", characterStats:heroStats, sword);
        
        var resolver = new CombatResolver();

        var rand_part = enemy.Body.GetRandomPart();

        var startEffectiveness = rand_part.Effectiveness; // currently 100 in random

        // Act
        var afterFirstAttack = resolver.ResolveAttack_SpecificPart(hero, enemy, rand_part);
        var afterSecondAttack = resolver.ResolveAttack_SpecificPart(hero, enemy, rand_part);

        // Assert (persistence: the second call builds on the state changed by the first)
        Assert.Equal(startEffectiveness - hero.Weapon!.Damage, afterFirstAttack);
        Assert.Equal(startEffectiveness - (2 * hero.Weapon!.Damage), afterSecondAttack);

        // Also assert the defender's state actually changed (not just the return value)
        Assert.Equal(afterSecondAttack, rand_part.Effectiveness);
    }
}