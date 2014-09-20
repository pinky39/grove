namespace Grove.Triggers
{
  using Grove.Events;
  using Grove.Infrastructure;

  public class OnLevelChanged : Trigger, IReceive<LevelChangedEvent>
  {
    private readonly int _level;

    private OnLevelChanged() {}

    public OnLevelChanged(int level)
    {
      _level = level;
    }

    public void Receive(LevelChangedEvent message)
    {
      if (message.Card == Ability.OwningCard)
      {
        if (message.Card.Level == _level)
        {
          Set();
        }
      }
    }
  }
}