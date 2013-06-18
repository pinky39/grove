namespace Grove.Gameplay.Messages
{
  using System;

  public class PlayerPlayedALand
  {
    public Card Card { get; private set; }

    public PlayerPlayedALand(Card card)
    {
      Card = card;
    }

    public override string ToString()
    {
      return String.Format("{0} played {1}", Card.Controller, Card);
    }
  }
}