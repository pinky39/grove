namespace Grove.Effects
{
  using Decisions;
  using Modifiers;

  public class PayManaApplyToCard : Effect, IProcessDecisionResults<BooleanResult>
  {
    private readonly DynParam<IManaAmount> _amount;
    private readonly CardModifierFactory _modifier;    
    private readonly string _message;

    private PayManaApplyToCard() {}

    public PayManaApplyToCard(IManaAmount amount, CardModifierFactory modifier, string message = null)
      : this(new DynParam<IManaAmount>(amount), modifier, message) {}

    public PayManaApplyToCard(DynParam<IManaAmount> amount, CardModifierFactory modifier, string message = null)
    {
      _amount = amount;
      _modifier = modifier;
      _message = message ?? "Pay mana?";

      RegisterDynamicParameters(amount);
    }

    public void ProcessResults(BooleanResult results)
    {
      if (!results.IsTrue)
        return;

      var p = new ModifierParameters
      {
        SourceEffect = this,
        SourceCard = Source.OwningCard,
        X = X
      };

      Source.OwningCard.AddModifier(_modifier(), p);
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
