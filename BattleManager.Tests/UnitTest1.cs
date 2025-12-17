using TextBasedGame;
using TextBasedGame.Characters;
using TextBasedGame.Equipment.Weapons;
using Xunit;

namespace BattleManager.Tests;

public class UnitTest1
{
    [Fact]
    public void ResolveAttack_TwiceAgainstSameDefender_PersistsDamageAcrossCalls()
    {
        // Arrange
        var enemy = new GenericTestCharacter("Orc", new Sword());
        var hero = new TestHero("Halfdan", new Sword());
        var resolver = new CombatResolver();

        var startEffectiveness = enemy.Body.Head.Effectiveness; // currently 100 in Head

        // Act
        var afterFirstAttack = resolver.ResolveAttack(hero, enemy);
        var afterSecondAttack = resolver.ResolveAttack(hero, enemy);

        // Assert (persistence: the second call builds on the state changed by the first)
        Assert.Equal(startEffectiveness - hero.Weapon!.Damage, afterFirstAttack);
        Assert.Equal(startEffectiveness - (2 * hero.Weapon!.Damage), afterSecondAttack);

        // Also assert the defender's state actually changed (not just the return value)
        Assert.Equal(afterSecondAttack, enemy.Body.Head.Effectiveness);
    }
}