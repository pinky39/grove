namespace Grove.Core.Cards.Preventions
{
  public class PreventDealt : DamagePrevention
  {        
    public override int PreventDealtDamage(int amount)
    {
      return 0;
    }
  }
}