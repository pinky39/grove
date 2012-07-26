namespace Grove.Core.Controllers.Results
{
  using Infrastructure;

  [Copyable]
  public class ChosenPlayer
  {
    public ChosenPlayer(Player player)
    {
      Player = player;
    }

    public Player Player { get; private set; }    
  }
}