namespace Grove.Gameplay.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Modifiers;
  using Targeting;
  using Zones;

  public class ApplyModifiersToSelfAndToTargets : Effect
  {
    private readonly List<ModifierFactory> _selfModifiers = new List<ModifierFactory>();
    private readonly List<ModifierFactory> _targetModifiers = new List<ModifierFactory>();

    private readonly Value _toughnessReductionSelf;
    private readonly Value _toughnessReductionTargets;

    private ApplyModifiersToSelfAndToTargets() {}

    public ApplyModifiersToSelfAndToTargets(IEnumerable<ModifierFactory> self,
      IEnumerable<ModifierFactory> target, Value toughnessReductionSelf = null,
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

      var selfModifiers = CreateSelfModifiers();

      foreach (var modifier in selfModifiers)
      {
        Source.OwningCard.AddModifier(modifier);
      }

      var targetModifiers = CreateTargetModifiers();

      foreach (var modifier in targetModifiers)
      {
        Target.Card().AddModifier(modifier);
      }
    }

    private IEnumerable<Modifier> CreateSelfModifiers()
    {
      var p = new ModifierParameters
        {
          SourceEffect = this,
          SourceCard = Source.OwningCard,
          Target = Source.OwningCard,
          X = X
        };


      return _selfModifiers.Select(factory =>
        {
          var modifier = factory();
          modifier.Initialize(p, Game);
          return modifier;
        });
    }

    private IEnumerable<Modifier> CreateTargetModifiers()
    {
      var p = new ModifierParameters
        {
          SourceEffect = this,
          SourceCard = Source.OwningCard,
          Target = Target,
          X = X
        };


      return _targetModifiers.Select(factory =>
        {
          var modifier = factory();
          modifier.Initialize(p, Game);
          return modifier;
        });
    }
  }
}