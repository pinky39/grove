namespace Grove.Core.Costs
{
  using System;
  using Grove.Core.Targeting;

  public class PayLife : Cost
  {
    public int Amount;
    public Func<PayLife, int> GetAmount = self => self.Amount;
    
    public override bool CanPay(ref int? maxX)
    {
      return GetAmount(this) <= Card.Controller.Life;
    }

    protected override void Pay(ITarget target, int? x)
    {
      Card.Controller.Life -= GetAmount(this);
    }
  }
}