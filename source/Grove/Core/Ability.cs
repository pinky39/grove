namespace Grove.Core
{
  using Ai;
  using Effects;
  using Infrastructure;
  using Zones;

  [Copyable]
  public abstract class Ability : IEffectSource
  {
    private readonly TargetSelectors _targetSelectors = new TargetSelectors();
    public TargetSelectors TargetSelectors {get { return _targetSelectors; }}
    
    public Player Controller { get { return OwningCard.Controller; } }        
    protected IEffectFactory EffectFactory { get; private set; }
    protected Game Game { get; set; }
    protected Publisher Publisher {get { return Game.Publisher; }}    
    public Card SourceCard { get; protected set; }
    protected Stack Stack { get { return Game.Stack; } }    
    public CardText Text { get; set; }
    public EffectCategories EffectCategories { get; set; }
    public Card OwningCard { get; protected set; }
    public bool UsesStack { get; set; }
    public abstract int CalculateHash(HashCalculator calc);

    public void EffectWasCountered() {}

    void IEffectSource.EffectWasPushedOnStack() {}
    void IEffectSource.EffectWasResolved() {}

    bool IEffectSource.AreTargetsStillValid(Targets targets)
    {
      return TargetSelectors.AreTargetsStillValid(targets);
    }

    public void Effect(IEffectFactory effectFactory)
    {
      EffectFactory = effectFactory;
    }

    public void SetTargetSelector(string name, ITargetSelectorFactory factory)
    {
      TargetSelectors[name] = factory.Create(OwningCard);
    }
    
    public void SetEffectSelector(ITargetSelectorFactory factory)
    {
      TargetSelectors.Effect = factory.Create(OwningCard);
    }

    public void SetCostSelector(ITargetSelectorFactory factory)
    {
      TargetSelectors.Cost = factory.Create(OwningCard);
    }

    public void SetTargetsFilter(TargetsFilterDelegate filter)
    {
      TargetSelectors.Filter = filter;
    }

    public override string ToString()
    {
      return string.Format("{0}'s ability", OwningCard);
    }
  }
}