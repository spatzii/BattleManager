
using TextBasedGame.Characters;
using TextBasedGame.Characters.CharacterStats;
using TextBasedGame.Equipment.EquipmentStats.WeaponStats;
using TextBasedGame.DamageMechanics.Body;
using TextBasedGame.Equipment.Weapons;

namespace TextBasedGame;

internal static class Program
{
    public static void Main(string[] args)
    {

        var sword = Sword.Create();
        var club = Club.Create();
        
        var bowieStats = CharacterLoader.LoadStatsFromFile(GamePaths.CHARACTER_STATS + "TestDefaultHero.json");
        var player = new TestCharacter("Bowie", bowieStats, sword);

        var ogreStats = CharacterLoader.LoadStatsFromFile(GamePaths.CHARACTER_STATS + "Ogre.json");
        var enemy = new TestCharacter("Ogre", ogreStats, club);

        var battle = new BattleMenu(player, enemy);
        battle.StartBattle();

    }
}