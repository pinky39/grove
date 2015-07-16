namespace Grove.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Modifiers;

  public class Attach : Effect
  {
    private readonly List<ModifierFactory> _modifiers = new List<ModifierFactory>();

    private Attach() {}

    public Attach(params ModifierFactory[] modifiers)
    {
      _modifiers.AddRange(modifiers);
    }

    public override int CalculateToughnessReduction(Card card)
    {
      if (Target == card)
      {
        return ToughnessReduction.GetValue(X);
      }
      return 0;
    }

    protected override void ResolveEffect()
    {
      // Fix: when equipment becomes creature (e.g Ensoul Artifact, Haunted Plate Mail) it tries to equip self. It caused StackOverflow exception
      if (Source.OwningCard.Is().Creature && Source.OwningCard.Is().Equipment)
        return;

      var attachTo = (Card) Target;

      attachTo.Attach(Source.OwningCard);
      
      var p = new ModifierParameters
        {
          SourceEffect = this,
          SourceCard = Source.OwningCard,
          X = X
        };

      foreach (var modifier in _modifiers.Select(factory => factory()))
      {
        if (modifier is ICardModifier)
        {
          attachTo.AddModifier((ICardModifier) modifier, p);
        }
        else if (modifier is IGameModifier)
        {
          Game.AddModifier((IGameModifier) modifier, p);
        }
      }
    }
  }
}