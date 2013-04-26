namespace Grove.Gameplay.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Ai;
  using Card;
  using Card.Characteristics;
  using Common;
  using Infrastructure;
  using Messages;
  using Modifiers;
  using Player;
  using Targeting;

  public delegate Effect EffectFactory();

  public abstract class Effect : GameObject, ITarget, IHasColors
  {
    private readonly List<IDynamicParameter> _dynamicParameters = new List<IDynamicParameter>();
    private readonly Trackable<bool> _wasResolved = new Trackable<bool>();

    public Action<Effect> AfterResolve = delegate { };
    public Action<Effect> BeforeResolve = delegate { };
    public bool CanBeCountered = true;
    public EffectCategories Category = EffectCategories.Generic;
    public Func<Effect, bool> ShouldResolve = delegate { return true; };
    public Value ToughnessReduction = 0;
    private object _triggerMessage;
    public Player Controller { get { return Source.OwningCard.Controller; } }
    public IEffectSource Source { get; private set; }
    public int? X { get; private set; }
    private bool WasResolved { get { return _wasResolved.Value; } set { _wasResolved.Value = value; } }
    public Targets Targets { get; private set; }

    public virtual bool TargetsEffectSource { get { return false; } }

    public ITarget Target
    {
      get
      {
        if (Targets.Effect.Count > 0)
          return Targets.Effect[0];

        if (Targets.Cost.Count > 0)
          return Targets.Cost[0];

        return null;
      }
    }

    public IEnumerable<ITarget> ValidEffectTargets
    {
      get
      {
        if (Targets.Effect.Count == 1)
        {
          // if there is only one target it must be valid
          // otherwise the spell would not resolve
          // so no need to check it again

          yield return Target;
          yield break;
        }

        foreach (var valid in Targets.Effect.Where(IsValid))
        {
          yield return valid;
        }
      }
    }

    public bool HasColor(CardColor color)
    {
      return Source.OwningCard.HasColor(color);
    }

    public int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        GetType().GetHashCode(),
        calc.Calculate(Source),
        calc.Calculate(Targets),
        CanBeCountered.GetHashCode(),
        X.GetHashCode());
    }

    protected void RegisterDynamicParameters(params IDynamicParameter[] parameters)
    {
      foreach (var dynamicParameter in parameters)
      {
        if (dynamicParameter != null)
          _dynamicParameters.Add(dynamicParameter);
      }
    }

    protected virtual void Initialize() {}

    public T TriggerMessage<T>()
    {
      return (T) _triggerMessage;
    }

    public bool IsValid(ITarget target)
    {
      return Source.IsTargetStillValid(target, _triggerMessage);
    }

    public virtual int CalculatePlayerDamage(Player player)
    {
      return 0;
    }

    public virtual int CalculateCreatureDamage(Card creature)
    {
      return 0;
    }

    public virtual int CalculateToughnessReduction(Card card)
    {
      return 0;
    }

    public void EffectCountered(SpellCounterReason reason)
    {
      Source.EffectCountered(reason);
      OnEffectCountered(reason);
    }

    protected virtual void OnEffectCountered(SpellCounterReason reason) {}

    public void EffectWasPushedOnStack()
    {
      Source.EffectPushedOnStack();
    }

    public void FinishResolve()
    {
      if (WasResolved)
      {
        Source.EffectResolved();

        Publish(new EffectResolved {Effect = this});
        return;
      }

      EffectCountered(SpellCounterReason.IllegalTarget);
    }

    public bool CanBeResolved()
    {
      return Targets.Effect.None() ||
        Targets.Effect.Any(IsValid);
    }

    public override string ToString()
    {
      return Source.ToString();
    }

    public void Resolve()
    {
      BeforeResolve(this);

      if (ShouldResolve(this))
      {
        foreach (var parameter in _dynamicParameters)
        {
          parameter.EvaluateOnResolve(this, Game);
        }

        ResolveEffect();
        AfterResolve(this);
      }

      WasResolved = true;
    }

    protected abstract void ResolveEffect();

    public bool HasCategory(EffectCategories effectCategories)
    {
      return ((effectCategories & Category) != EffectCategories.Generic);
    }

    public bool HasTarget(Card card)
    {
      return Targets.Any(x => x == card);
    }

    public virtual Effect Initialize(EffectParameters p, Game game, bool initializeDynamicParameters = true)
    {
      Game = game;
      Source = p.Source;
      Targets = p.Targets ?? new Targets();
      _triggerMessage = p.TriggerMessage;
      X = p.X;

      _wasResolved.Initialize(game.ChangeTracker);

      if (initializeDynamicParameters)
      {
        foreach (var parameter in _dynamicParameters)
        {
          parameter.EvaluateOnInit(this, Game);
        }
      }

      Initialize();

      return this;
    }
  }
}