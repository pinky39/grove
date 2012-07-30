namespace Grove.Core.Details.Cards.Effects
{
  using System.Collections.Generic;
  using Modifiers;
  using Targeting;
  using Zones;

  public class ApplyModifiersToSelfAndToTargets : Effect
  {
    private readonly List<IModifierFactory> _selfModifiers = new List<IModifierFactory>();
    private readonly List<IModifierFactory> _targetModifiers = new List<IModifierFactory>();

    public Value ToughnessReductionSelf = 0;
    public Value ToughnessReductionTargets = 0;

    public override int CalculateToughnessReduction(Card creature)
    {
      if (creature == Source.OwningCard)
        return ToughnessReductionSelf.GetValue(X);

      if (creature == Target())
      {
        return ToughnessReductionTargets.GetValue(X);
      }

      return 0;
    }

    public override bool NeedsTargets
    {
      get { return true; }
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
        Target().Card().AddModifier(modifier);
      }
    }

    public void SelfModifiers(params IModifierFactory[] modifiers)
    {
      _selfModifiers.AddRange(modifiers);
    }

    public void TargetModifiers(params IModifierFactory[] modifiers)
    {
      _targetModifiers.AddRange(modifiers);
    }

    private IEnumerable<Modifier> CreateSelfModifiers()
    {
      return _selfModifiers.CreateModifiers(Source.OwningCard, Source.OwningCard, X);
    }

    private IEnumerable<Modifier> CreateTargetModifiers()
    {
      return _targetModifiers.CreateModifiers(Source.OwningCard, Target().Card(), X);
    }
  }
}