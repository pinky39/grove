namespace Grove.Core.Zones
{
  using System.Collections.Generic;

  public interface IHandQuery : IZoneQuery
  {
    int Count { get; }
    IEnumerable<Card> Lands { get; }
  }
}