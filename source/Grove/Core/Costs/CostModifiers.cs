namespace Grove.Costs
{
  using Grove.Infrastructure;
  using Modifiers;

  [Copyable]
  public class CostModifiers : IAcceptsGameModifier
  {
    private readonly TrackableList<CostModifier> _costModifiers = new TrackableList<CostModifier>();

    public void Accept(IGameModifier modifier)
    {
      modifier.Apply(this);
    }

    public void Initialize(ChangeTracker changeTracker)
    {
      _costModifiers.Initialize(changeTracker);
    }

    public void Add(CostModifier costModifier)
    {
      _costModifiers.Add(costModifier);
    }

    public void Remove(CostModifier costModifier)
    {
      _costModifiers.Remove(costModifier);
    }

    public IManaAmount GetActualCost(IManaAmount amount, ManaUsage usage, Card card)
    {
      var actualCost = amount;

      foreach (var costModifier in _costModifiers)
      {
        actualCost = costModifier.GetActualCost(actualCost, usage, card);
      }

      return actualCost;
    }
  }
}