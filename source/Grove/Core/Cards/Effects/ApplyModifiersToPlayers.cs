namespace Grove.Core.Cards.Effects
{
  using System.Collections.Generic;
  using Modifiers;

  public class ApplyModifiersToPlayer : Effect
  {
    private readonly List<IModifierFactory> _modifierFactories = new List<IModifierFactory>();
    public Player Player;

    protected override void ResolveEffect()
    {
      foreach (var modifier in _modifierFactories.CreateModifiers(Source.OwningCard, Player, X, Game))
      {
        Player.AddModifier(modifier);
      }
    }

    public void Modifiers(params IModifierFactory[] modifiers)
    {
      _modifierFactories.AddRange(modifiers);
    }
  }
}