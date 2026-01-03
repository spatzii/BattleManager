using TextBasedGame.Characters;

namespace TextBasedGame;

public class RoundManager(ICharacter hero, ICharacter enemy)
{
    private readonly ICharacter _hero = hero;
    private readonly ICharacter _enemy = enemy;

    public bool CanContinueCombat()
    {
        return IsCharacterAlive(_hero);
    }

    private bool IsCharacterAlive(ICharacter character)
    {
        return true;
    }

    public static void Attack(ICharacter attacker, ICharacter defender)
    {
        var damageRound = new DamageCalculator(attacker, defender).Calculate();
        // var armorAfterAttack = _enemy.Stats.Evasion- damageRound.NetAdvantage;
        Console.WriteLine($"{attacker.Name} did {damageRound.RawAttackValue} to " +
                          $"{defender.Name}, meaning {damageRound.NormalizedValue} normalized value.");
        attacker.GameState.ConsumeStamina(10);
        Console.WriteLine($"{attacker.Name} has {attacker.GameState.CurrentStamina} stamina left.");
}
}