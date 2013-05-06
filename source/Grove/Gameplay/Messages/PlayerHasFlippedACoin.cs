namespace Grove.Gameplay.Messages
{
  public class PlayerHasFlippedACoin
  {
    public Player Player { get; set; }
    public bool HasWon { get; set; }

    public override string ToString()
    {
      return string.Format("{0} {1} the coin flip.", Player, HasWon ? "won" : "lost");
    }
  }
}