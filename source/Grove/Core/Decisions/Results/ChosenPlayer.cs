namespace Grove.Core.Decisions.Results
{
  using Infrastructure;

  [Copyable]
  public class ChosenPlayer
  {
    private ChosenPlayer() {}

    public ChosenPlayer(Player player)
    {
      Player = player;
    }

    public Player Player { get; private set; }
  }
}