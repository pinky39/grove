namespace Grove.Core.Modifiers
{
  public class IncreaseLandLimit : Modifier
  {
    private readonly int _amount;
    private readonly Increment _increment = new Increment();
    private LandLimit _landLimit;

    private IncreaseLandLimit() {}

    public IncreaseLandLimit(int amount = 1)
    {
      _amount = amount;
    }

    public override void Apply(LandLimit landLimit)
    {
      _landLimit = landLimit;
      _increment.Initialize(ChangeTracker);
      _increment.Value = _amount;
      _landLimit.AddModifier(_increment);
    }

    protected override void Unapply()
    {
      _landLimit.RemoveModifier(_increment);
    }
  }
}