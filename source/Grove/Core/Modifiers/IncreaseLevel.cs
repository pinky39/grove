namespace Grove.Core.Modifiers
{
  using Messages;

  public class IncreaseLevel : Modifier
  {
    private Level _level;
    private Increment _levelIncrement;

    public override void Apply(Level level)
    {
      _level = level;
      _levelIncrement = new Increment(1, ChangeTracker);
      _level.AddModifier(_levelIncrement);
      

      Publisher.Publish(new CardChangedLevel
        {
          Card = (Card)Target,
        });
    }

    protected override void Unapply()
    {
      _level.RemoveModifier(_levelIncrement);
    }
  }
}