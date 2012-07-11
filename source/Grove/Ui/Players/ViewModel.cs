namespace Grove.Ui.Players
{
  using Castle.Core;
  using Core;
  using Infrastructure;

  [Transient]
  public class ViewModel : IReceive<SelectionModeChanged>
  {
    private readonly Game _game;
    private readonly Publisher _publisher;


    public ViewModel(Game game, Publisher publisher)
    {
      _game = game;
      _publisher = publisher;
    }

    public virtual bool CanSelectPlayer { get; protected set; }

    public Players Players { get { return _game.Players; } }

    public void Receive(SelectionModeChanged message)
    {
      CanSelectPlayer = message.SelectionMode == SelectionMode.SelectTarget;
    }

    public void SelectPlayer(Player player)
    {
      _publisher.Publish(
        new TargetSelected {Target = player});
    }
  }
}