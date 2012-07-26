namespace Grove.Ui.ManaPool
{
  using Core;

  public class ViewModel
  {    
    public ViewModel(Game game)
    {
      Player = game.Players.Human;
    }

    public Player Player { get; private set; }
  }
}