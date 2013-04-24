namespace Grove.Core.Zones
{
  using System;

  public interface ILibraryQuery : IZoneQuery
  {
    event EventHandler Shuffled;
  }
}