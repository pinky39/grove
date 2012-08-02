namespace Grove.Core.Zones
{
  using System.Collections.Generic;

  public interface IGraveyardQuery : IEnumerable<Card>
  {
    IEnumerable<Card> Creatures { get; }
  }
}