namespace Grove.Core.Cards.Modifiers
{
  public class IncreaseLandLimit : Modifier
  {
    public int Amount = 1;
    private Increment _increment;
    private LandLimit _landLimit;

    public override void Apply(LandLimit landLimit)
    {
      _landLimit = landLimit;
      _increment = new Increment(Amount, ChangeTracker);
      _landLimit.AddModifier(_increment);
    }

    protected override void Unapply()
    {
      _landLimit.RemoveModifier(_increment);
    }
  }
}