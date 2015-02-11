namespace Grove.Costs
{
  using Infrastructure;
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

    public int GetCostChange(CostType type, Card card)
    {
      var change = 0;

      foreach (var costModifier in _costModifiers)
      {
        change += costModifier.GetChange(type, card);
      }

      return change;
    }
  }
}