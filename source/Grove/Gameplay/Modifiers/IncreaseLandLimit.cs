namespace Grove.Core.Modifiers
{
  public class IncreaseLandLimit : Modifier
  {    
    private readonly Increment _increment;
    private LandLimit _landLimit;

    private IncreaseLandLimit() {}

    public IncreaseLandLimit(int amount = 1)
    {
      _increment = new Increment(amount);
    }

    public override void Apply(LandLimit landLimit)
    {
      _landLimit = landLimit;
      _increment.Initialize(ChangeTracker);      
      _landLimit.AddModifier(_increment);
    }

    protected override void Unapply()
    {
      _landLimit.RemoveModifier(_increment);
    }
  }
}