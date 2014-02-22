namespace Grove.Gameplay.Messages
{
  public class TypeChanged
  {
    public Card Card { get; set; }
    public CardType OldValue { get; set; }
    public CardType NewValue { get; set; }    
  }
}