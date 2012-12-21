namespace Grove.Core.Zones
{
  using System.Collections.Generic;

  public interface IGraveyardQuery : IZoneQuery
  {
    IEnumerable<Card> Creatures { get; }
  }
}