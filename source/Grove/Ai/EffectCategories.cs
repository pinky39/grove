namespace Grove.Ai
{
  using System;

  [Flags]
  public enum EffectCategories
  {
    Generic = 0,
    Destruction = 2,
    Bounce = 4,
    Counterspell = 8,
    ToughnessIncrease = 16,
    Protector = 32,
    Exile = 64,
  }
}