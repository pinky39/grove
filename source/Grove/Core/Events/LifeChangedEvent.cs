namespace Grove.Events
{
  public class LifeChangedEvent
  {
    public readonly Player Player;

    public LifeChangedEvent(Player player)
    {
      Player = player;
    }

    public override string ToString()
    {
      return string.Format("{0} life is now {1}.", Player.Name, Player.Life);
    }
  }
}