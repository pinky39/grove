namespace Grove.Core.Effects
{
  using System;
  using Grove.Core.Decisions;
  using Grove.Core.Decisions.Results;
  using Grove.Core.Mana;

  public class PayManaOrSacrifice : Effect, IProcessDecisionResults<BooleanResult>
  {
    public string Message = "Pay {0}?";
    public IManaAmount Amount { get; set; }


    public void ResultProcessed(BooleanResult results)
    {
      if (results.IsTrue)
        return;

      Source.OwningCard.Sacrifice();
    }

    protected override void ResolveEffect()
    {
      Game.Enqueue<PayOr>(Controller, p =>
        {
          p.ManaAmount = Amount;
          p.Text = FormatText(String.Format("Pay {0}?", Amount));
        });
    }
  }
}