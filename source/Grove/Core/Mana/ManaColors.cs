namespace Grove.Core.Mana
{
  using System;

  [Flags]
  public enum ManaColors
  {
    None = 0,
    White = 1,
    Blue = 2,
    Black = 4,
    Red = 8,
    Green = 16,
    Colorless = 32,
    All = White | Blue | Black | Red | Green
  }
}