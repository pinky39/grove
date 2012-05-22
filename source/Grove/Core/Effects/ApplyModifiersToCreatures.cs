namespace Grove.Core.Effects
{
  using System.Collections.Generic;
  using Modifiers;

  public class ApplyModifiersToCreatures : Effect
  {
    private readonly List<IModifierFactory> _modifierFactories = new List<IModifierFactory>();

    public void Modifiers(params IModifierFactory[] modifiers)
    {
      _modifierFactories.AddRange(modifiers);
    }

    public override void Resolve()
    {
      if (HasTarget)
      {
        ApplyModifierToPlayersCreatures(Target.Player());
        return;
      }

      foreach (var player in Players)
      {
        ApplyModifierToPlayersCreatures(player);
      }
    }

    private void ApplyModifierToPlayersCreatures(Player player)
    {
      foreach (var creature in player.Battlefield.Creatures)
      {
        foreach (var modifier in _modifierFactories.CreateModifiers(Source.OwningCard, creature, X))
        {
          creature.AddModifier(modifier);
        }
      }
    }
  }
}