namespace Grove.Core.Details.Cards.Effects
{
  using System;

  public class GainLife : Effect
  {
    public int Amount { get; set; }

    protected override void ResolveEffect()
    {
      Controller.Life += Amount;
    }
  }
}