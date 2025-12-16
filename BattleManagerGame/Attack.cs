using TextBasedGame.Characters;
using TextBasedGame.DamageMechanics.BodyParts;

namespace TextBasedGame;

public class Attack
{
    public void AttackBodyPart(IBodyParts bodyPart, int damage)
    {
        // Math.Max(0...) clamps the effectiveness to zero. Remove if we implement spillover
        bodyPart.Effectiveness = Math.Max(0, bodyPart.Effectiveness - damage);
        Console.WriteLine($"{bodyPart.Name} took {damage} damage, and it has" +
                          $" {bodyPart.Effectiveness} effectiveness left.");
    }
}

public class RoundGenerator
{
    public void Round(ICharacter player1, ICharacter player2)
    {
    }
}

