namespace Grove.Gameplay
{
  using System;

  public interface ILibraryQuery : IZoneQuery
  {
    event EventHandler Shuffled;
  }
}