namespace Grove.Core.Modifiers
{
  public class IncreaseLandLimit : Modifier
  {
    private readonly int _amount;
    private Increment _increment;
    private LandLimit _landLimit;

    private IncreaseLandLimit() {}

    public IncreaseLandLimit(int amount = 1)
    {
      _amount = amount;
    }

    public override void Apply(LandLimit landLimit)
    {
      _landLimit = landLimit;
      _increment = new Increment(_amount, ChangeTracker);
      _landLimit.AddModifier(_increment);
    }

    protected override void Unapply()
    {
      _landLimit.RemoveModifier(_increment);
    }
  }
}