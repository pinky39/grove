namespace Grove.Gameplay.Zones
{
  using System.Collections.Generic;

  public interface IGraveyardQuery : IZoneQuery
  {
    IEnumerable<Card> Creatures { get; }
  }
}