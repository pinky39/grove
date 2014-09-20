namespace Grove.UserInterface.TurnNumber
{
  using Events;
  using Infrastructure;

  public class ViewModel : ViewModelBase, IReceive<TurnStartedEvent>
  {
    public virtual int Number { get; protected set; }

    public void Receive(TurnStartedEvent message)
    {
      Number = message.TurnCount;
    }

    public override void Initialize()
    {
      Number = Game.Turn.TurnCount;
    }
  }
}