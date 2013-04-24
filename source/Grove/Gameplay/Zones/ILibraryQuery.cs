namespace Grove.Gameplay.Zones
{
  using System;

  public interface ILibraryQuery : IZoneQuery
  {
    event EventHandler Shuffled;
  }
}