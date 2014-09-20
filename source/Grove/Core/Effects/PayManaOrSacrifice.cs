namespace Grove.Effects
{
  using Grove.Decisions;

  public class PayManaOrSacrifice : Effect, IProcessDecisionResults<BooleanResult>
  {
    private readonly DynParam<IManaAmount> _amount;
    private readonly string _message;
    private PayManaOrSacrifice() {}

    public PayManaOrSacrifice(IManaAmount amount, string message = null)
      : this(new DynParam<IManaAmount>(amount), message) {}

    public PayManaOrSacrifice(DynParam<IManaAmount> amount, string message = null)
    {
      _amount = amount;
      _message = message ?? "Pay mana?";

      RegisterDynamicParameters(amount);
    }

    public void ProcessResults(BooleanResult results)
    {
      if (results.IsTrue)
        return;
      
      Source.OwningCard.Sacrifice();
    }

    protected override void ResolveEffect()
    {
      Enqueue(new PayOr(Controller, p =>
        {
          p.ManaAmount = _amount.Value;
          p.Text = _message;
          p.ProcessDecisionResults = this;
          p.ManaUsage = ManaUsage.Abilities;
        }));
    }
  }
}