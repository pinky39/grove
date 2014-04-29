namespace Grove.Events
{
  using Effects;

  public class SpellCounteredEvent
  {
    public readonly Card Card;
    public readonly SpellCounterReason Reason;

    public SpellCounteredEvent(Card card, SpellCounterReason reason)
    {
      Card = card;
      Reason = reason;
    }
  }
}