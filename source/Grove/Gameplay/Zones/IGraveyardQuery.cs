namespace Grove.Gameplay.Zones
{
  using System.Collections.Generic;
  using Card;

  public interface IGraveyardQuery : IZoneQuery
  {
    IEnumerable<Card> Creatures { get; }
  }
}