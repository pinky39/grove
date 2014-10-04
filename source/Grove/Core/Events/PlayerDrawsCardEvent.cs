namespace Grove.Events
{
  public class PlayerDrawsCardEvent
  {
    public readonly Player Player;
    
    public PlayerDrawsCardEvent(Player player)
    {
      Player = player;
    }
  }
}
