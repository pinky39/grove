namespace Grove.Core.Ai
{
  using System;

  [Flags]
  public enum EffectCategories
  {
    Generic = 0,
    DamageDealing = 1,
    PwTReduction = 2,
    Destruction = 4,
    Bounce = 8,
    Counterspell = 16,    
    Removal = DamageDealing | PwTReduction | Destruction | Bounce,
    LifepointReduction = PwTReduction | DamageDealing,    
  } 
}