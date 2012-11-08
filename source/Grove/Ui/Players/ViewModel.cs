namespace Grove.Ui.Players
{
  using Core;
  using Infrastructure;

  public class ViewModel : IReceive<UiInteractionChanged>
  {
    private readonly Game _game;

    public ViewModel(Game game)
    {
      _game = game;
    }

    public virtual bool CanChangeSelection { get; protected set; }

    public Players Players { get { return _game.Players; } }

    public void Receive(UiInteractionChanged message)
    {
      CanChangeSelection = message.State == InteractionState.SelectTarget;
    }

    public void ChangeSelection(Player player)
    {
      _game.Publish(
        new SelectionChanged {Selection = player});
    }
  }
}