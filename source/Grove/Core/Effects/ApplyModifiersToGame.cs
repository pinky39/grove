namespace Grove.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Modifiers;

  public class ApplyModifiersToGame : Effect
  {
    private readonly List<GameModifierFactory> _modifiers = new List<GameModifierFactory>();

    private ApplyModifiersToGame() {}

    public ApplyModifiersToGame(params GameModifierFactory[] modifiers)
    {
      _modifiers.AddRange(modifiers);
    }

    protected override void ResolveEffect()
    {
      var p = new ModifierParameters
        {
          SourceEffect = this,
          SourceCard = Source.OwningCard,
          X = X
        };

      foreach (var modifier in _modifiers.Select(modifierFactory => modifierFactory()))
      {
        Game.AddModifier(modifier, p);
      }
    }
  }
}