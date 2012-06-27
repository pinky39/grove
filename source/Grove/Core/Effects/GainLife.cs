namespace Grove.Core.Effects
{
  using System;

  public class GainLife : Effect
  {
    public Func<GainLife, int> Amount = delegate { return 0; };

    public void SetAmount(int value)
    {
      Amount = delegate { return value; };
    }
    
    protected override void ResolveEffect()
    {
      Controller.Life += Amount(this);
    }    
  }
}