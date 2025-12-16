// See https://aka.ms/new-console-template for more information



// var member = new Torso();
// var attack = new Attack();
// attack.AttackBodyPart(member, 10);

using TextBasedGame.Characters;

namespace TextBasedGame;

internal static class Program
{
    public static void Main(string[] args)
    {
        var enemy = new GenericTestCharacter();
        var attack = new Attack();
        attack.AttackBodyPart(enemy.Body.Torso, 15);
        attack.AttackBodyPart(enemy.Body.Torso, 10);
        
    }
}