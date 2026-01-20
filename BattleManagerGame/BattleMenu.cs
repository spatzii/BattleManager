using TextBasedGame.Characters;
using TextBasedGame.Combat;
using TextBasedGame.DamageMechanics.Body;

namespace TextBasedGame;

public class BattleMenu
{
    private readonly ICharacter _player;
    private readonly ICharacter _enemy;
    private bool _battleOver;
    private bool _debugFlag;
    private bool _printToFile;

    public BattleMenu(ICharacter player, ICharacter enemy)
    {
        _player = player;
        _enemy = enemy;
        Console.WriteLine($"\n⚔️  {_enemy.Name} appears!\n");
    }

    public void StartBattle()
    {
        
        while (!_battleOver)
        {
            DisplayBattleState();
            DisplayMenu();
            ProcessPlayerChoice();
            
            if (!_battleOver)
                ProcessEnemyTurn();
            
            CheckBattleEnd();
        }
    }

    private void DisplayBattleState()
    {
        Console.WriteLine("═══════════════════════════════════════");
        Console.WriteLine($"  {_player.Name}: {_player.GameState.HealthState}");
        Console.WriteLine($"  {_enemy.Name}: {_enemy.GameState.HealthState}");
        Console.WriteLine("═══════════════════════════════════════");
    }

    private static void DisplayMenu()
    {
        Console.WriteLine("\n  [A]ttack  |  [D]efend  |  [P]otions  |  [F]lee | [O]ptions;");
        Console.Write("  Your action: ");
    }

    private void ProcessPlayerChoice()
    {
        var input = Console.ReadKey(intercept: true).KeyChar;
        Console.WriteLine(input);
        
        switch (char.ToLower(input))
        {
            case 'a':
                DisplayTargetMenu();
                break;
            case 'd':
                ExecuteDefend();
                break;
            case 'p':
                OpenPotionMenu();
                break;
            case 'o':
                Options();
                break;
            case 'f':
                AttemptFlee();
                break;
            default:
                Console.WriteLine("  Unknown command.");
                break;
        }
    }

    private void Options()
    {
        var inOptions = true;
        while (inOptions)
        {

            Console.WriteLine(
                $"\n [D]ebug: {_debugFlag.ToString()} | [P]rint to file: {_printToFile.ToString()} | [E]xit \n");
            Console.Write("  Your action: ");

            var input = Console.ReadKey(intercept: true).KeyChar;
            Console.WriteLine(input);

            switch (char.ToLower(input))
            {
                case 'd':
                    DebugFlipper();
                    break;
                case 'p':
                    PrintFlipper();
                    break;
                case 'e':
                    inOptions = false;
                    break;
                default:
                    Console.WriteLine("  Unknown command.");
                    break;
            }
        }
    }

    private void DisplayTargetMenu()
    {
        Console.WriteLine("\n  Select target:");
        Console.WriteLine("  [1] Head");
        Console.WriteLine("  [2] Torso");
        Console.WriteLine("  [3] Left Arm");
        Console.WriteLine("  [4] Right Arm");
        Console.WriteLine("  [5] Left Leg");
        Console.WriteLine("  [6] Right Leg");
        Console.WriteLine("  [R] Random");
        Console.WriteLine("  [C] Cancel");
        Console.Write("\n  Target: ");

        var input = Console.ReadKey(intercept: true).KeyChar;
        Console.WriteLine(input);

        switch (char.ToLower(input))
        {
            case '1':
                ExecutePlayerAttack(BodyPartType.Head);
                break;
            case '2':
                ExecutePlayerAttack(BodyPartType.Torso);
                break;
            case '3':
                ExecutePlayerAttack(BodyPartType.LeftArm);
                break;
            case '4':
                ExecutePlayerAttack(BodyPartType.RightArm);
                break;
            case '5':
                ExecutePlayerAttack(BodyPartType.LeftLeg);
                break;
            case '6':
                ExecutePlayerAttack(BodyPartType.RightLeg);
                break;
            case 'r':
                ExecutePlayerAttack(null);
                break;
            case 'c':
                return;
            default:
                Console.WriteLine("  Invalid target.");
                return;
        }
    }

    private void ExecutePlayerAttack(BodyPartType? targetType)
    {
        var targetPart = targetType.HasValue
            ? _enemy.Body.GetPart(targetType.Value)
            : _enemy.Body.GetRandomPart();

        Console.WriteLine($"\n  You strike at the {_enemy.Name}'s {targetPart.Name}!");
        _player.ResolveAttackAgainst(_enemy, targetPart, showDebug: _debugFlag, printDebug: _printToFile);
    }

    private void ExecuteDefend()
    {
        // Placeholder: could set a "defending" flag that modifies incoming damage
        Console.WriteLine("\n  You raise your guard...");
    }

    private void OpenPotionMenu()
    {
        // Placeholder: submenu for inventory
        Console.WriteLine("\n  You rummage through your pack... (not implemented)");
    }

    private void AttemptFlee()
    {
        // Placeholder: could be stat-based chance
        Console.WriteLine("\n  You turn and flee!");
        _battleOver = true;
    }

    private void ProcessEnemyTurn()
    {
        if (_enemy.GetOverallCondition() <= 0)
            return;
            
        Console.WriteLine($"\n  The {_enemy.Name} retaliates!");
        _enemy.ResolveAttackAgainst(_player, _player.Body.GetRandomPart(), showDebug: _debugFlag, printDebug: _printToFile);
    }

    private void CheckBattleEnd()
    {
        if (_enemy.GetOverallCondition() <= 0)
        {
            Console.WriteLine($"\n  ☠️  The {_enemy.Name} falls!");
            _battleOver = true;
        }
        else if (_player.GetOverallCondition() <= 0)
        {
            Console.WriteLine($"\n  💀  {_player.Name} has been slain...");
            _battleOver = true;
        }
    }

    private void DebugFlipper()
    {
        _debugFlag = !_debugFlag;
    }

    private void PrintFlipper()
    {
        _printToFile = !_printToFile;
    }
}