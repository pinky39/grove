namespace Grove.Gameplay.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Modifiers;

  public class ApplyModifiersToSelfAndToTargets : Effect
  {
    private readonly List<CardModifierFactory> _selfModifiers = new List<CardModifierFactory>();
    private readonly List<CardModifierFactory> _targetModifiers = new List<CardModifierFactory>();

    private readonly Value _toughnessReductionSelf;
    private readonly Value _toughnessReductionTargets;

    private ApplyModifiersToSelfAndToTargets() {}

    public ApplyModifiersToSelfAndToTargets(
      CardModifierFactory self,
      CardModifierFactory target,
      Value toughnessReductionSelf = null,
      Value toughnessReductionTargets = null)
    {
      _selfModifiers.Add(self);
      _targetModifiers.Add(target);

      _toughnessReductionSelf = toughnessReductionSelf ?? 0;
      _toughnessReductionTargets = toughnessReductionTargets ?? 0;
    }

    public ApplyModifiersToSelfAndToTargets(
      IEnumerable<CardModifierFactory> self,
      IEnumerable<CardModifierFactory> target,
      Value toughnessReductionSelf = null,
      Value toughnessReductionTargets = null)
    {
      _selfModifiers.AddRange(self);
      _targetModifiers.AddRange(target);

      _toughnessReductionSelf = toughnessReductionSelf ?? 0;
      _toughnessReductionTargets = toughnessReductionTargets ?? 0;
    }

    public override int CalculateToughnessReduction(Card card)
    {
      if (card == Source.OwningCard)
        return _toughnessReductionSelf.GetValue(X);

      if (card == Target)
      {
        return _toughnessReductionTargets.GetValue(X);
      }

      return 0;
    }

    protected override void ResolveEffect()
    {
      if (Source.OwningCard.Zone != Zone.Battlefield)
        return;

      var p = new ModifierParameters
        {
          SourceEffect = this,
          SourceCard = Source.OwningCard,
          X = X
        };

      var selfModifiers = _selfModifiers.Select(factory => factory());

      foreach (var modifier in selfModifiers)
      {
        Source.OwningCard.AddModifier(modifier, p);
      }

      var targetModifiers = _targetModifiers.Select(factory => factory());

      foreach (var modifier in targetModifiers)
      {
        Target.Card().AddModifier(modifier, p);
      }
    }
  }
}