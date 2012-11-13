namespace Grove.Core.Decisions.Results
{
  using Grove.Infrastructure;

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