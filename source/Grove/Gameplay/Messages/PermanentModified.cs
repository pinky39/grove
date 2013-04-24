namespace Grove.Gameplay.Messages
{
  using Card;
  using Modifiers;

  public class PermanentWasModified
  {
    public Card Card { get; set; }
    public IModifier Modifier { get; set; }
  }
}