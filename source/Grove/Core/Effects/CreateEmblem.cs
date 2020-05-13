namespace Grove.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Modifiers;

  public class CreateEmblem : Effect
  {
    private readonly string _text;
    private readonly int _score;
    private readonly DynParam<Player> _controller;
    private readonly List<ModifierFactory> _modifiers = new List<ModifierFactory>();

    private CreateEmblem() {}

    public CreateEmblem(
      string text,
      int score,
      DynParam<Player> controller,
      params ModifierFactory[] modifiers
      )
    {
      _text = text;
      _score = score;
      _controller = controller;
      _modifiers.AddRange(modifiers);

      RegisterDynamicParameters(controller);
    }

    protected override void ResolveEffect()
    {      
      var emblem = new Emblem(_text, _score);      
      
      var p = new ModifierParameters
        {
          SourceEffect = this,
          SourceCard = Source.OwningCard,
          X = X
        };

      foreach (var modifier in _modifiers.Select(modifierFactory => modifierFactory()))
      {
        var lifetime = new EmblemLifetime(emblem);
        lifetime.Initialize(Game);
        
        modifier.AddLifetime(lifetime);

        if (modifier is IGameModifier gameModifier)
        {
          Game.AddModifier(gameModifier, p);
        }
        else if (modifier is IPlayerModifier playerModifier)
        {
          _controller.Value.AddModifier(playerModifier, p);
        }
      }

      _controller.Value.AddEmblem(emblem);
    }
  }
}