namespace Grove.Gameplay.Zones
{
  using System.Collections.Generic;

  public interface IHandQuery : IZoneQuery
  {
    IEnumerable<Card> Lands { get; }
  }
}