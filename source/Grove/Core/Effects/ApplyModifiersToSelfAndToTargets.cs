namespace Grove.Core.Effects
{
  using System.Collections.Generic;
  using Grove.Core.Targeting;
  using Grove.Core.Zones;
  using Modifiers;

  public class ApplyModifiersToSelfAndToTargets : Effect
  {
    private readonly List<IModifierFactory> _selfModifiers = new List<IModifierFactory>();
    private readonly List<IModifierFactory> _targetModifiers = new List<IModifierFactory>();

    public Value ToughnessReductionSelf = 0;
    public Value ToughnessReductionTargets = 0;

    public override int CalculateToughnessReduction(Card card)
    {
      if (card == Source.OwningCard)
        return ToughnessReductionSelf.GetValue(X);

      if (card == Target())
      {
        return ToughnessReductionTargets.GetValue(X);
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
      return _selfModifiers.CreateModifiers(Source.OwningCard, Source.OwningCard, X, Game);
    }

    private IEnumerable<Modifier> CreateTargetModifiers()
    {
      return _targetModifiers.CreateModifiers(Source.OwningCard, Target().Card(), X, Game);
    }
  }
}