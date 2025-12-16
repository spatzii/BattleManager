using TextBasedGame.DamageMechanics.Body;

namespace TextBasedGame.Characters;

public class GenericTestCharacter : ICharacter
{
    public string Name => "Generic Enemy";
    private readonly IBody _body = new Body();
    public IBody Body => _body;
    
}