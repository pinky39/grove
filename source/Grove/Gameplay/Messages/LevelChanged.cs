namespace Grove.Gameplay.Messages
{
  using Card;

  public class TypeChanged
  {
    public Card Card { get; set; }
  }
  
  public class LevelChanged
  {
    public Card Card { get; set; }
  }
}