namespace Grove.Gameplay.Damage
{
  using System;

  [Serializable]
  public class PreventCombatDamage : DamagePrevention
  {
    public override int PreventDealtCombatDamage(int amount)
    {
      return 0;
    }

    public override int EvaluateDealtCombatDamage(int amount)
    {
      return 0;
    }
  }
}