namespace Grove.Core.Messages
{
  using Details.Cards.Modifiers;

  public class PermanentWasModified
  {
    public Card Card { get; set; }
    public IModifier Modifier { get; set; }
  }
}