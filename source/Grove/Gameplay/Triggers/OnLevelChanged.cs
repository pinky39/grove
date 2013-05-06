namespace Grove.Gameplay.Triggers
{
  using Infrastructure;
  using Messages;

  public class OnLevelChanged : Trigger, IReceive<LevelChanged>
  {
    private readonly int _level;

    private OnLevelChanged() {}

    public OnLevelChanged(int level)
    {
      _level = level;
    }

    public void Receive(LevelChanged message)
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