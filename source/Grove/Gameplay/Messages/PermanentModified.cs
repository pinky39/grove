namespace Grove.Core.Messages
{
  using Modifiers;

  public class PermanentWasModified
  {
    public Card Card { get; set; }
    public IModifier Modifier { get; set; }
  }
}