namespace Grove.Core.Details.Mana
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
  }
}