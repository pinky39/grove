namespace Grove.Core.Cards.Triggers
{
  using Infrastructure;
  using Messages;

  public class OnLevelChanged : Trigger, IReceive<LevelChanged>
  {
    public int Level { get; set; }

    public void Receive(LevelChanged message)
    {
      if (message.Card == Ability.OwningCard)
      {
        if (message.Card.Level == Level)
        {
          Set();
        }
      }
    }
  }
}