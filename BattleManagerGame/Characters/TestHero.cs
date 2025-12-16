using TextBasedGame.DamageMechanics.Body;

namespace TextBasedGame.Characters;

public class TestHero : ICharacter
{
    public string Name => "Our hero";
    private readonly IBody _body = new Body();
    public IBody Body => _body;
}