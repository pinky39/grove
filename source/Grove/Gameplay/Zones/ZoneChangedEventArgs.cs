namespace Grove.Gameplay.Zones
{
  using System;
  using Card;

  public class ZoneChangedEventArgs : EventArgs
  {
    public Card Card { get; private set; }
    public int? Index { get; private set; }

    public ZoneChangedEventArgs(Card card, int? index = null)
    {
      Card = card;
      Index = index;
    }
  }
}