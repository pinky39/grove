namespace Grove.Gameplay
{
  using System;

  [Flags]
  public enum ManaUsage
  {
    None = 0,
    Spells = 1,
    Abilities = 2,
    Any = Spells | Abilities
  }
}