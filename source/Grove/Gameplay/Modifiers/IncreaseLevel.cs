namespace Grove.Gameplay.Modifiers
{
  using Characteristics;

  public class IncreaseLevel : Modifier
  {
    private readonly Increment _levelIncrement = new Increment(1);
    private Level _level;

    public override void Apply(Level level)
    {
      _level = level;
      _levelIncrement.Initialize(ChangeTracker);
      _level.AddModifier(_levelIncrement);
    }

    protected override void Unapply()
    {
      _level.RemoveModifier(_levelIncrement);
    }
  }
}