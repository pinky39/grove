namespace Grove.Gameplay.Zones
{
  using System;

  public class ZoneChangedEventArgs : EventArgs
  {
    public ZoneChangedEventArgs(Card card, int? index = null)
    {
      Card = card;
      Index = index;
    }

    public Card Card { get; private set; }
    public int? Index { get; private set; }
  }
}