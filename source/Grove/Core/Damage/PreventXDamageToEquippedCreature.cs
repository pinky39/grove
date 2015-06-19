namespace Grove
{
  using System;

  public class PreventXDamageToEquippedCreature : DamagePrevention
  {    
    private readonly Func<Context, int> _amount;

    private PreventXDamageToEquippedCreature() {}

    public PreventXDamageToEquippedCreature(Func<Context, int> amount)
    {      
      _amount = amount;
    }

    public override int PreventDamage(PreventDamageParameters p)
    {
      return p.Target == Modifier.SourceCard.AttachedTo 
        ? _amount(Ctx) 
        : 0;
    }
  }
}