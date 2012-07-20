namespace Grove.Core.Controllers.Results
{
  using Infrastructure;

  [Copyable]
  public class ChosenPlayer
  {
    public ChosenPlayer(IPlayer player)
    {
      Player = player;
    }

    public IPlayer Player { get; private set; }    
  }
}