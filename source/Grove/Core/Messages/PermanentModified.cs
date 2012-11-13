namespace Grove.Core.Messages
{
  using Cards.Modifiers;

  public class PermanentWasModified
  {
    public Card Card { get; set; }
    public IModifier Modifier { get; set; }
  }
}