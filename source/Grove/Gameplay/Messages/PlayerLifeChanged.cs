namespace Grove.Gameplay.Messages
{
  public class PlayerLifeChanged
  {
    public Player Player { get; set; }

    public override string ToString()
    {
      return string.Format("{0} life is now {1}.", Player.Name, Player.Life);
    }
  }
}