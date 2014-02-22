namespace Grove.Gameplay
{
  using System.Collections.Generic;

  public interface IGraveyardQuery : IZoneQuery
  {
    IEnumerable<Card> Creatures { get; }
  }
}