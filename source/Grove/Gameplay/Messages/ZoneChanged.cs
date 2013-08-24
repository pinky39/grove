namespace Grove.Gameplay.Messages
{
  using System;
  using Zones;

  public class ZoneChanged
  {
    public Card Card { get; set; }
    public Player Controller { get { return Card.Controller; } }
    public Zone From { get; set; }
    public bool FromBattlefield { get { return From == Zone.Battlefield; } }
    public bool FromBattlefieldToGraveyard { get { return From == Zone.Battlefield && To == Zone.Graveyard; } }
    public Zone To { get; set; }
    public bool ToBattlefield { get { return To == Zone.Battlefield; } }
    public bool ToHand {get { return To == Zone.Hand; }}
    public bool FromHand { get { return From == Zone.Hand; } }
  }
}