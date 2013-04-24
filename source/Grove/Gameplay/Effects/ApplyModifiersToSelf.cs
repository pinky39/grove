namespace Grove.Gameplay.Effects
{
  using System.Collections.Generic;
  using Card;
  using Modifiers;
  using Zones;

  public class ApplyModifiersToSelf : Effect
  {
    private readonly List<ModifierFactory> _selfModifiers = new List<ModifierFactory>();    

    private ApplyModifiersToSelf() {}
    
    public ApplyModifiersToSelf(params ModifierFactory[] modifiers)
    {      
      _selfModifiers.AddRange(modifiers);
    }

    public override bool TargetsEffectSource { get { return true; } }

    public override int CalculateToughnessReduction(Card card)
    {
      return card == Source.OwningCard ? ToughnessReduction.GetValue(X) : 0;
    }

    private IEnumerable<Modifier> CreateSelfModifiers()
    {
      var target = Source.OwningCard;

      foreach (var modifierFactory in _selfModifiers)
      {
        var p = new ModifierParameters
          {
            SourceEffect = this,
            SourceCard = Source.OwningCard,
            Target = target,
            X = X
          };

        yield return modifierFactory().Initialize(p, Game);
      }
    }

    protected override void ResolveEffect()
    {
      var target = Source.OwningCard;

      if (Source.OwningCard.Zone == Zone.Battlefield)
      {
        foreach (var modifier in CreateSelfModifiers())
        {
          target.AddModifier(modifier);
        }
      }
    }
  }
}