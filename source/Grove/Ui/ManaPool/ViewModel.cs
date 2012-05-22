namespace Grove.Ui.ManaPool
{
  using Core;
  using Infrastructure;

  public class ViewModel
  {
    private readonly Player _player;

    public ViewModel(Game game)
    {
      _player = game.Players.Human;
      _player.Property(x => x.ManaPool).Changes(this).Property(x => x.ManaPool);
    }

    public ManaAmount ManaPool
    {
      get { return _player.ManaPool; }
    }   
  }
}