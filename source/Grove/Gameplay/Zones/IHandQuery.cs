namespace Grove.Core.Zones
{
  using System.Collections.Generic;

  public interface IHandQuery : IZoneQuery
  {    
    IEnumerable<Card> Lands { get; }
  }
}