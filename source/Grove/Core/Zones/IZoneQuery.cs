namespace Grove.Core.Zones
{
  using System;
  using System.Collections.Generic;

  public interface IZoneQuery : IEnumerable<Card>
  {
    event EventHandler<ZoneChangedEventArgs> CardAdded;
    event EventHandler<ZoneChangedEventArgs> CardRemoved;
  }
}