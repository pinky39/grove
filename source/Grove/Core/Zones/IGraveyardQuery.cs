namespace Grove
{
  using System.Collections.Generic;

  public interface IGraveyardQuery : IZoneQuery
  {
    IEnumerable<Card> Creatures { get; }
  }
}