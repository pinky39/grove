namespace Grove.Events
{
  public class PlayerTookMulliganEvent
  {
    public readonly Player Player;

    public PlayerTookMulliganEvent(Player player)
    {
      Player = player;
    }
  }
}