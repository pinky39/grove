namespace Grove.Core
{
  using Ai;
  using Effects;
  using Infrastructure;
  using Zones;

  [Copyable]
  public abstract class Ability : IEffectSource, IHashable
  {
    public Player Controller { get { return OwningCard.Controller; } }
    protected IEffectFactory EffectFactory { get; private set; }
    protected Game Game { get; set; }
    protected Publisher Publisher {get { return Game.Publisher; }}
    protected bool NeedsTarget { get { return TargetSelector != null; } }
    public Card SourceCard { get; protected set; }
    protected Stack Stack { get { return Game.Stack; } }
    protected TargetSelector TargetSelector { get; set; }
    public CardText Text { get; set; }
    public EffectCategories EffectCategories { get; set; }
    public Card OwningCard { get; protected set; }
    public bool UsesStack { get; set; }
    public abstract int CalculateHash(HashCalculator calc);

    public void EffectWasCountered() {}

    void IEffectSource.EffectWasPushedOnStack() {}
    void IEffectSource.EffectWasResolved() {}

    bool IEffectSource.IsTargetValid(ITarget target)
    {
      return TargetSelector.IsValid(target);
    }

    public void Effect(IEffectFactory effectFactory)
    {
      EffectFactory = effectFactory;
    }

    public void SetTargetSelector(ITargetSelectorFactory targetSelectorFactory)
    {
      TargetSelector = targetSelectorFactory.Create(OwningCard);
    }

    public override string ToString()
    {
      return string.Format("{0}'s ability", OwningCard);
    }
  }
}