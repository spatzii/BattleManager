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

    private static bool IsCharacterAlive(ICharacter character)
    {
        return character.GetOverallCondition() > 0;
    }

    public void Attack(ICharacter attacker, ICharacter defender)
    {
        if (IsCharacterAlive(defender))
        {
            attacker.ResolveAttackAgainst(defender, defender.Body.GetRandomPart(), showDebug: true);
        }
        else
        {
            Console.WriteLine($"{defender.Name} is dead!");
        }
        
    }

}