namespace Grove.Core.Details.Cards.Preventions
{
  public class PreventDealtDamage : DamagePrevention
  {
    public override int PreventDealtCombatDamage(int amount)
    {
      return 0;
    }
  }
}