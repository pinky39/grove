namespace Grove.Core.Controllers.Results
{
  using Infrastructure;

  [Copyable]
  public class ChosenPlayer
  {
    public Player Player { get; set; }

    public static implicit operator ChosenPlayer(Player player)
    {
      return new ChosenPlayer {Player = player};
    }
  }
}