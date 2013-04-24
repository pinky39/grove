﻿namespace Grove.Gameplay.Card.Costs
{
  using Targeting;

  public class Reveal : Cost
  {
    protected override void CanPay(CanPayResult result)
    {
      result.CanPay = Card.Controller.Hand.Count > 0;
    }

    protected override void Pay(ITarget target, int? x)
    {
      target.Card().Reveal();
    }
  }
}