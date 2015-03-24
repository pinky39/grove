namespace Grove.Effects
{
  using System;
  using Decisions;

  public class PayManaThen : PayThen
  {
    private readonly DynParam<ManaAmount> _amount;

    private PayManaThen() {}

    public PayManaThen(DynParam<ManaAmount> amount, Effect effect, Parameters parameters = null)
      : base(effect, parameters ?? new Parameters {Message = "Pay mana?"})
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
    private PayLifeThen() {}

    private readonly DynParam<int> _amount;

    public PayLifeThen(DynParam<int> amount, Effect effect, Parameters parameters = null)
      : base(effect, parameters ?? new Parameters {Message = "Pay life?"})
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
    private readonly Func<Effect, bool> _aiPaysIf;

    protected PayThen() {}

    protected PayThen(Effect effect, Parameters p)
    {
      _effect = effect;
      _execIfPaid = p.ExecuteIfPaid;
      _message = p.Message;
      _aiPaysIf = p.AiPaysIf;
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

    public BooleanResult ChooseResult()
    {
      return _aiPaysIf(this);
    }

    public class Parameters
    {
      public string Message = null;
      public bool ExecuteIfPaid = true;
      public Func<Effect, bool> AiPaysIf = (e) => true;
    }
  }
}