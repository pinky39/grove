namespace Grove.Gameplay.Modifiers
{
  using Characteristics;

  public class IncreaseLandLimit : Modifier, IPlayerModifier
  {
    private readonly IntegerIncrement _integerIncrement;
    private LandLimit _landLimit;

    private IncreaseLandLimit() {}

    public IncreaseLandLimit(int amount = 1)
    {
      _integerIncrement = new IntegerIncrement(amount);
    }

    public override void Apply(LandLimit landLimit)
    {
      _landLimit = landLimit;
      _integerIncrement.Initialize(ChangeTracker);
      _landLimit.AddModifier(_integerIncrement);
    }

    protected override void Unapply()
    {
      _landLimit.RemoveModifier(_integerIncrement);
    }
  }
}