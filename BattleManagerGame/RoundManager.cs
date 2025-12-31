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
}