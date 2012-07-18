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
    private readonly TargetSelectors _targetSelectors = new TargetSelectors();
    public TargetSelectors TargetSelectors { get { return _targetSelectors; } }

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

    bool IEffectSource.AreTargetsStillValid(IList<ITarget> targets, bool wasKickerPaid)
    {
      return TargetSelectors.AreValidEffectTargets(targets);
    }

    public void Effect(IEffectFactory effectFactory)
    {
      EffectFactory = effectFactory;
    }

    public void EffectTargets(params ITargetSelectorFactory[] factories)
    {
      foreach (var factory in factories)
      {
        TargetSelectors.AddEffectSelector(factory.Create(OwningCard));
      }
    }

    public void CostTargets(params ITargetSelectorFactory[] factories)
    {
      foreach (var factory in factories)
      {
        TargetSelectors.AddCostSelector(factory.Create(OwningCard));
      }
    }

    public void TargetsFilter(TargetsFilterDelegate filter)
    {
      TargetSelectors.Filter = filter;
    }

    public override string ToString()
    {
      return string.Format("{0}'s ability", OwningCard);
    }
  }
}