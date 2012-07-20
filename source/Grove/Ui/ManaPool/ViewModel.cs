namespace Grove.Ui.ManaPool
{
  using Core;

  public class ViewModel
  {    
    public ViewModel(Game game)
    {
      Player = game.Players.Human;
    }

    public IPlayer Player { get; private set; }
  }
}