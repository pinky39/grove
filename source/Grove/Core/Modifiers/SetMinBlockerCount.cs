namespace Grove.Modifiers
{    
  public class SetMinBlockerCount : Modifier, ICardModifier
  {
    private readonly IntegerSetter _integerSetter;
    private MinimumBlockerCount _minimumBlockerCount;

    private SetMinBlockerCount() {}

    public SetMinBlockerCount(int amount)
    {
      _integerSetter = new IntegerSetter(amount);
    }

    public override void Apply(MinimumBlockerCount count)
    {
      _minimumBlockerCount = count;
      _integerSetter.Initialize(ChangeTracker);
      _minimumBlockerCount.AddModifier(_integerSetter);
    }

    protected override void Unapply()
    {
      _minimumBlockerCount.RemoveModifier(_integerSetter);
    }
  }
}
