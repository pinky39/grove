namespace Grove.Events
{
  using System;

  public class LandPlayedEvent
  {
    public readonly Card Card;

    public LandPlayedEvent(Card card)
    {
      Card = card;
    }

    public override string ToString()
    {
      return String.Format("{0} played {1}", Card.Controller, Card);
    }
  }
}