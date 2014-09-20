namespace Grove.Events
{
  using Modifiers;

  public class PermanentModifiedEvent
  {
    public readonly Card Card;
    public readonly IModifier Modifier;

    public PermanentModifiedEvent(Card card, IModifier modifier)
    {
      Card = card;
      Modifier = modifier;
    }
  }
}