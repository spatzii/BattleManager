using TextBasedGame.Characters;

namespace TextBasedGame;

public class BattleMenu
{
    private readonly ICharacter _player;
    private readonly ICharacter _enemy;
    private bool _battleOver;

    public BattleMenu(ICharacter player, ICharacter enemy)
    {
        _player = player;
        _enemy = enemy;
    }

    public void StartBattle()
    {
        Console.WriteLine($"\n⚔️  {_enemy.Name} appears!\n");
        
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

    private void DisplayMenu()
    {
        Console.WriteLine("\n  [A]ttack  |  [D]efend  |  [P]otions  |  [F]lee\n");
        Console.Write("  Your action: ");
    }

    private void ProcessPlayerChoice()
    {
        var input = Console.ReadKey(intercept: true).KeyChar;
        Console.WriteLine(input);
        
        switch (char.ToLower(input))
        {
            case 'a':
                ExecutePlayerAttack();
                break;
            case 'd':
                ExecuteDefend();
                break;
            case 'p':
                OpenPotionMenu();
                break;
            case 'f':
                AttemptFlee();
                break;
            default:
                Console.WriteLine("  Unknown command.");
                break;
        }
    }

    private void ExecutePlayerAttack()
    {
        Console.WriteLine($"\n  You strike at the {_enemy.Name}!");
        _player.ResolveAttackAgainst(_enemy, _enemy.Body.GetRandomPart(), showDebug: false);
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
        _enemy.ResolveAttackAgainst(_player, _player.Body.GetRandomPart(), showDebug: false);
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
}