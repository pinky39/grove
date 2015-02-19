namespace Grove.Effects
{
  using Decisions;
  using Modifiers;

  public class PayManaApplyToCard : Effect, IProcessDecisionResults<BooleanResult>
  {
    private readonly DynParam<ManaAmount> _amount;
    private readonly CardModifierFactory _modifier;    
    private readonly string _message;

    private PayManaApplyToCard() {}

    public PayManaApplyToCard(ManaAmount amount, CardModifierFactory modifier, string message = null)
      : this(new DynParam<ManaAmount>(amount), modifier, message) {}

    public PayManaApplyToCard(DynParam<ManaAmount> amount, CardModifierFactory modifier, string message = null)
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

      var card = (Target == null || Target.Card() == null)
        ? Source.OwningCard
        : Target.Card();

      card.AddModifier(_modifier(), p);
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
