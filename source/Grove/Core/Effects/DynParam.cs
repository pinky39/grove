namespace Grove.Effects
{
  using System;
  using Infrastructure;

  public interface IDynamicParameter
  {
    void EvaluateOnResolve(Effect effect, Game game);
    void EvaluateOnInit(Effect effect, Game game);
    void EvaluateAfterCost(Effect effect, Game game);
    void EvaluateAfterTriggeredAbilityTargets(Effect effect, Game game);
    void Initialize(ChangeTracker changeTracker);
  }

  public enum EvaluateAt
  {
    OnResolve,
    OnInit,
    AfterTriggeredAbilityTargets,
    AfterCost
  }

  [Copyable]
  public class DynParam<TOut> : IDynamicParameter
  {    
    private readonly Func<Effect, Game, TOut> _getter;
    private readonly EvaluateAt _evaluateAt;
    private readonly Trackable<TOut> _value;
    private bool _isInitialized;

    public DynParam(Func<Effect, Game, TOut> getter, EvaluateAt evaluateAt = EvaluateAt.OnInit)
    {
      _getter = getter;
      _evaluateAt = evaluateAt;

      _value = new Trackable<TOut>();
    }

    private DynParam() {}

    public DynParam(TOut value)
    {
      _value = new Trackable<TOut>(value);
      _isInitialized = true;
    }

    public TOut Value
    {
      get
      {
        Asrt.True(_isInitialized,
          "Parameter was not initialized, did you forget to register it?");

        return _value;
      }
    }

    public void EvaluateAfterTriggeredAbilityTargets(Effect effect, Game game)
    {
      if (_evaluateAt == EvaluateAt.AfterTriggeredAbilityTargets)
      {
        Evaluate(effect, game);
      }
    }

    public void Initialize(ChangeTracker changeTracker)
    {
      _value.Initialize(changeTracker);
    }

    public void EvaluateOnResolve(Effect effect, Game game)
    {
      if (_evaluateAt == EvaluateAt.OnResolve)
      {
        Evaluate(effect, game);
      }
    }

    public void EvaluateOnInit(Effect effect, Game game)
    {
      if (_evaluateAt == EvaluateAt.OnInit || _evaluateAt == EvaluateAt.OnResolve)
      {
        Evaluate(effect, game);
      }
    }

    public void EvaluateAfterCost(Effect effect, Game game)
    {
      if (_evaluateAt == EvaluateAt.AfterCost)
      {
        Evaluate(effect, game);
      }
    }

    private void Evaluate(Effect effect, Game game)
    {
      if (_getter != null)
        _value.Value = _getter(effect, game);

      _isInitialized = true;
    }

    public static implicit operator DynParam<TOut>(Func<Effect, Game, TOut> getter)
    {
      return new DynParam<TOut>(getter);
    }

    public static implicit operator DynParam<TOut>(Func<Effect, TOut> getter)
    {
      return new DynParam<TOut>((e, g) => getter(e));
    }

    public static implicit operator DynParam<TOut>(TOut value)
    {
      return new DynParam<TOut>(value);
    }

    public static implicit operator TOut(DynParam<TOut> param)
    {
      return param.Value;
    }
  }
}