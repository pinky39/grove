namespace Grove.Modifiers
{
  public class IncreaseMinBlockerCount : Modifier, ICardModifier
  {
    private readonly IntegerIncrement _integerIncrement;
    private MinimumBlockerCount _minimumBlockerCount;

    private IncreaseMinBlockerCount() {}

    public IncreaseMinBlockerCount(int amount = 1)
    {
      _integerIncrement = new IntegerIncrement(amount);
    }

    public override void Apply(MinimumBlockerCount count)
    {
      _minimumBlockerCount = count;
      _integerIncrement.Initialize(ChangeTracker);
      _minimumBlockerCount.AddModifier(_integerIncrement);
    }

    protected override void Unapply()
    {
      _minimumBlockerCount.RemoveModifier(_integerIncrement);
    }
  }
}
