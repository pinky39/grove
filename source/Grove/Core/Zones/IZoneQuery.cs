namespace Grove
{
  using System;
  using System.Collections.Generic;

  public interface IZoneQuery : IEnumerable<Card>
  {
    int Count { get; }
    event EventHandler<ZoneChangedEventArgs> CardAdded;
    event EventHandler<ZoneChangedEventArgs> CardRemoved;
  }
}