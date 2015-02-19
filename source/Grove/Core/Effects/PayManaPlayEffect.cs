namespace Grove.Effects
{
  using Decisions;

  public class PayManaPlayEffect : Effect, IProcessDecisionResults<BooleanResult>
  {
    private readonly DynParam<ManaAmount> _amount;
    private readonly Effect _effect;    
    private readonly string _message;

    private PayManaPlayEffect() {}   

    public PayManaPlayEffect(DynParam<ManaAmount> amount, Effect effect, string message = null)
    {
      _amount = amount;
      _effect = effect;
      _message = message ?? "Pay mana?";

      RegisterDynamicParameters(amount);
    }

    public override Effect Initialize(EffectParameters p, Game game, bool evaluateParameters = true)
    {
      base.Initialize(p, game);

      _effect.Initialize(p, game, evaluateParameters);

      return this;
    }

    public void ProcessResults(BooleanResult results)
    {
      if (!results.IsTrue)
        return;

      _effect.BeginResolve();
    }

      public override void SetTriggeredAbilityTargets(Targets targets)
    {
      _effect.SetTriggeredAbilityTargets(targets);
    }

    public override void FinishResolve()
    {
      if (WasResolved)
      {
        _effect.AfterResolve(_effect);
      }
      
      base.FinishResolve();
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
