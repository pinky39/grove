namespace Grove.Gameplay.Zones
{
  using System.Collections.Generic;
  using Card;

  public interface IHandQuery : IZoneQuery
  {    
    IEnumerable<Card> Lands { get; }
  }
}