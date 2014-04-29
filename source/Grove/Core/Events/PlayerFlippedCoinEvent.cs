namespace Grove.Events
{
  public class PlayerFlippedCoinEvent
  {
    public readonly bool HasWon;
    public readonly Player Player;

    public PlayerFlippedCoinEvent(Player player, bool hasWon)
    {
      Player = player;
      HasWon = hasWon;
    }

    public override string ToString()
    {
      return string.Format("{0} {1} the coin flip.", Player, HasWon ? "won" : "lost");
    }
  }
}