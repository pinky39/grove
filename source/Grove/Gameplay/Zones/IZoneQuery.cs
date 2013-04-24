namespace Grove.Gameplay.Zones
{
  using System;
  using System.Collections.Generic;
  using Card;

  public interface IZoneQuery : IEnumerable<Card>
  {
    event EventHandler<ZoneChangedEventArgs> CardAdded;
    event EventHandler<ZoneChangedEventArgs> CardRemoved;

    int Count { get; }
  }
}