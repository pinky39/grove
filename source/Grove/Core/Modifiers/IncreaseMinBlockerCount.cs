namespace Grove.Modifiers
{
  public class IncreaseMinBlockerCount : Modifier, ICardModifier
  {
    private readonly IntegerIncrement _integerIncrement;
    private MinBlockerCount _minBlockerCount;

    private IncreaseMinBlockerCount() {}

    public IncreaseMinBlockerCount(int amount = 1)
    {
      _integerIncrement = new IntegerIncrement(amount);
    }

    public override void Apply(MinBlockerCount count)
    {
      _minBlockerCount = count;
      _integerIncrement.Initialize(ChangeTracker);
      _minBlockerCount.AddModifier(_integerIncrement);
    }

    protected override void Unapply()
    {
      _minBlockerCount.RemoveModifier(_integerIncrement);
    }
  }
}
