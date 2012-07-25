namespace Grove.Core.Details.Cards.Modifiers
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
          Card = Target.Card(),
        });
    }

    protected override void Unapply()
    {
      _level.RemoveModifier(_levelIncrement);
    }
  }
}