
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

        // var bowieStats = CharacterLoader.LoadStatsFromFile(GamePaths.CHARACTER_STATS + "TestDefaultHero.json");                   
        // var player = new GenericTestCharacter("Bowie", characterStats: bowieStats, weapon: new Sword());
        // var acc = player.CharacterStats.Accuracy;
        // Console.WriteLine(acc);
        //
        // var swordStats = WeaponLoader.LoadStatsFromFile(GamePaths.WEAPON_STATS + "BasicSword.json");
        // var sword = new Sword(weaponStats: swordStats);
        // Console.WriteLine(sword.Name);
        // Console.WriteLine(sword.Damage);
        //

        var sword = Sword.Create();
        var club = Club.Create();
        
        var bowieStats = CharacterLoader.LoadStatsFromFile(GamePaths.CHARACTER_STATS + "TestDefaultHero.json");
        var player = new TestCharacter("Bowie", bowieStats, sword);

        var ogreStats = CharacterLoader.LoadStatsFromFile(GamePaths.CHARACTER_STATS + "Ogre.json");
        var enemy = new TestCharacter("Ogre", ogreStats, club);

        var dmg = new DamageCalculator(player, enemy).CalculateWithDebug();
        



    }
}