namespace Grove.Effects
{
  using Decisions;
    
  public class PayManaThen : PayThen
  {
    private readonly DynParam<ManaAmount> _amount;

    public PayManaThen(DynParam<ManaAmount> amount, Effect effect, bool execIfPaid = true, string message = null)
      :base(effect, execIfPaid, message ?? "Pay mana?")
    {
      _amount = amount;
      RegisterDynamicParameters(_amount);
    }

    protected override ManaAmount GetManaAmount()
    {
      return _amount;
    }

    protected override int? GetLifeAmount()
    {
      return null;
    }
  }

  public class PayLifeThen : PayThen
  {
    private readonly DynParam<int> _amount;

    public PayLifeThen(DynParam<int> amount, Effect effect, bool execIfPaid = true, string message = null)
      : base(effect, execIfPaid, message ?? "Pay life?")
    {
      _amount = amount;
      RegisterDynamicParameters(_amount);
    }

    protected override ManaAmount GetManaAmount()
    {
      return null;
    }

    protected override int? GetLifeAmount()
    {
      return _amount;
    }
  }
  
  public abstract class PayThen : Effect, IProcessDecisionResults<BooleanResult>, IChooseDecisionResults<BooleanResult>
  {    
    private readonly Effect _effect;
    private readonly bool _execIfPaid;
    private readonly string _message;

    private PayThen() {}

    protected PayThen(Effect effect, bool execIfPaid, string message)
    {
      
      _effect = effect;
      _execIfPaid = execIfPaid;
      _message = message;      
    }

    public override Effect Initialize(EffectParameters p, Game game, bool evaluateParameters = true)
    {
      base.Initialize(p, game);

      _effect.Initialize(p, game, evaluateParameters);

      return this;
    }

    public void ProcessResults(BooleanResult results)
    {
      if (_execIfPaid && !results.IsTrue)
        return;

      if (!_execIfPaid && results.IsTrue)
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

    protected abstract ManaAmount GetManaAmount();
    protected abstract int? GetLifeAmount();

    protected override void ResolveEffect()
    {
      Enqueue(new PayOr(Controller, p =>
        {
          p.ManaAmount = GetManaAmount();
          p.Life = GetLifeAmount();
          p.Text = _message;
          p.ProcessDecisionResults = this;
          p.ChooseDecisionResults = this;
          p.ManaUsage = ManaUsage.Abilities;
        }));
    }

    public virtual BooleanResult ChooseResult()
    {
      return true;
    }
  }
}
