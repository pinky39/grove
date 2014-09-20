namespace Grove
{
  using System;

  public interface ILibraryQuery : IZoneQuery
  {
    event EventHandler Shuffled;
  }
}