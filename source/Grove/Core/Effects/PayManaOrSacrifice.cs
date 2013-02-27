namespace Grove.Core.Effects
{
  using System;
  using Decisions;
  using Decisions.Results;
  using Mana;

  public class PayManaOrSacrifice : Effect, IProcessDecisionResults<BooleanResult>
  {
    private readonly IManaAmount _amount;
    private readonly string _message;
    private PayManaOrSacrifice() {}

    public PayManaOrSacrifice(IManaAmount amount, string message = null)
    {
      _amount = amount;
      _message = message ?? "Pay {0}?";
    }

    public void ProcessResults(BooleanResult results)
    {
      if (results.IsTrue)
        return;

      Source.OwningCard.Sacrifice();
    }

    protected override void ResolveEffect()
    {
      Enqueue<PayOr>(Controller, p =>
        {
          p.ManaAmount = _amount;
          p.Text = FormatText(String.Format(_message, _amount));
        });
    }
  }
}