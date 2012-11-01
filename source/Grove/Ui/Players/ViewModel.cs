namespace Grove.Ui.Players
{
  using Core;
  using Infrastructure;

  public class ViewModel : IReceive<UiInteractionChanged>
  {
    private readonly Players _players;
    private readonly Publisher _publisher;

    public ViewModel(Players players, Publisher publisher)
    {
      _players = players;
      _publisher = publisher;
    }

    public virtual bool CanChangeSelection { get; protected set; }

    public Players Players { get { return _players; } }

    public void Receive(UiInteractionChanged message)
    {
      CanChangeSelection = message.State == InteractionState.SelectTarget;
    }

    public void ChangeSelection(Player player)
    {
      _publisher.Publish(
        new SelectionChanged {Selection = player});
    }
  }
}