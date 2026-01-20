using System;
using TextBasedGame.Characters;
using TextBasedGame.Combat;

namespace TextBasedGame;

public class ObsoleteRoundManager(ICharacter hero, ICharacter enemy)
{
    private readonly ICharacter _hero = hero;
    private readonly ICharacter _enemy = enemy;

    private bool CanContinueCombat()
    {
        return IsCharacterAlive(_hero);
    }

    private static bool IsCharacterAlive(ICharacter character)
    {
        return character.GetOverallCondition() > 0;
    }

    private AttackerDefender Players()
    {
        var heroInitative = _hero.Stats.Initative;
        var enemyInitiative = _enemy.Stats.Initative;

        if (heroInitative > enemyInitiative)
        {
            return new AttackerDefender
            {
                Attacker = _hero,
                Defender = _enemy
            };
        }

        else
        {
            return new AttackerDefender
            {
                Attacker = _enemy,
                Defender = _hero
            };
        }

    }

    public void ExecuteAttack()
    {
        var attacker = Players().Attacker;
        var defender = Players().Defender;
        if (IsCharacterAlive(defender))
        {
            attacker.ResolveAttackAgainst(defender, defender.Body.GetRandomPart(), showDebug:true);
        }
        else
        {
            Console.WriteLine($"{defender.Name} is dead!");
        }

        if (IsCharacterAlive(attacker))
        {
            defender.ResolveAttackAgainst(attacker, attacker.Body.GetRandomPart(), showDebug:true);
        }
        else
        {
            Console.WriteLine($"{attacker.Name} is dead!");
        }
    }
}

public class AttackerDefender
{
    public required ICharacter Attacker { get; set; }
    public required ICharacter Defender { get; set; }
}