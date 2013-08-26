namespace Grove.Gameplay.Messages
{
  using Modifiers;

  public class PermanentWasModified
  {
    public Card Card { get; set; }
    public IModifier Modifier { get; set; }
  }
}