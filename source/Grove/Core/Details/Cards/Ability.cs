namespace Grove.Core.Details.Cards
{
  using System.Collections.Generic;
  using Ai;
  using Effects;
  using Infrastructure;
  using Targeting;
  using Zones;

  [Copyable]
  public abstract class Ability : IEffectSource
  {
    private readonly TargetSelector _targetSelector = new TargetSelector();
    public TargetSelector TargetSelector { get { return _targetSelector; } }

    public Player Controller { get { return OwningCard.Controller; } }
    protected IEffectFactory EffectFactory { get; private set; }
    protected Game Game { get; set; }
    protected Publisher Publisher { get { return Game.Publisher; } }
    public Card SourceCard { get; protected set; }
    protected Stack Stack { get { return Game.Stack; } }
    public CardText Text { get; set; }
    public bool UsesStack { get; set; }
    public EffectCategories EffectCategories { get; set; }
    public Card OwningCard { get; protected set; }
    public abstract int CalculateHash(HashCalculator calc);

    public void EffectWasCountered() {}

    void IEffectSource.EffectWasPushedOnStack() {}
    void IEffectSource.EffectWasResolved() {}

    bool IEffectSource.IsTargetStillValid(ITarget target, bool wasKickerPaid)
    {
      return TargetSelector.IsValidEffectTarget(target);
    }

    public void Effect(IEffectFactory effectFactory)
    {
      EffectFactory = effectFactory;
    }

    public void Targets(IEnumerable<ITargetValidatorFactory> effectValidators, IEnumerable<ITargetValidatorFactory> costValidators, AiTargetSelectorDelegate aiTargetFilter)
    {
      foreach (var factory in effectValidators)
      {
        TargetSelector.AddEffectSelector(factory.Create(OwningCard));
      }

      foreach (var factory in costValidators)
      {
        TargetSelector.AddCostSelector(factory.Create(OwningCard));
      }

      TargetSelector.AiSelector = aiTargetFilter;
    }

    public override string ToString()
    {
      return string.Format("{0}'s ability", OwningCard);
    }
  }
}