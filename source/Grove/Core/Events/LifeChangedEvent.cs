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
      var name = Player.Name == "You" ? "Your" : Player.Name;      
      return string.Format("{0} life total is {1}.", name, Player.Life);
    }
  }
}