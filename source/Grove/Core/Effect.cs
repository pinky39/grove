namespace Grove
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using Effects;
  using Events;
  using Infrastructure;
  using Modifiers;
  using Triggers;

  public delegate Effect EffectFactory();

  public abstract class Effect : GameObject, ITarget, IHasColors
  {
    private readonly List<IDynamicParameter> _dynamicParameters = new List<IDynamicParameter>();
    private readonly List<EffectTag> _tags = new List<EffectTag>();
    private readonly Trackable<bool> _wasResolved = new Trackable<bool>();    
    
    public Action<Effect> AfterResolve = delegate { };
    protected bool AllTargetsMustBeValid;
    public Action<Effect> BeforeResolve = delegate { };
    public bool CanBeCountered = true;
    public Func<Effect, bool> ShouldResolve = delegate { return true; };
    private Value _toughnessReduction = 0;
    public int TriggerOrderRule = TriggerOrder.Normal;

    private object _triggerMessage;

    public Value ToughnessReduction
    {
      get { return _toughnessReduction; }
      set
      {
        _toughnessReduction = value;
        SetTags(EffectTag.ReduceToughness);
      }
    }

    public Player Controller { get { return Source.OwningCard.Controller; } }
    public IEffectSource Source { get; private set; }
    public int? X { get; private set; }
    private bool WasResolved { get { return _wasResolved.Value; } set { _wasResolved.Value = value; } }
    public Targets Targets { get; private set; }
    public bool IsOnStack { get { return Stack.Contains(this); } }

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

    public CardColor[] Colors { get { return Source.OwningCard.Colors; } }

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

    public int Id { get; private set; }

    public bool HasEffectTargets()
    {
      return Targets.Effect.Count > 0;
    }

    public Effect SetTags(params EffectTag[] tags)
    {
      _tags.AddRange(tags);
      return this;
    }

    public IEnumerable<EffectTag> GetTags()
    {
      return _tags;
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
      foreach (var parameter in _dynamicParameters)
      {
        parameter.EvaluateAfterCost(this, Game);
      }

      Source.EffectPushedOnStack();
    }

    public void QuickResolve()
    {
      BeginResolve();
      FinishResolve();
    }

    public void FinishResolve()
    {
      if (WasResolved)
      {
        Source.EffectResolved();

        Publish(new EffectResolvedEvent(this));        
        return;
      }

      EffectCountered(SpellCounterReason.IllegalTarget);
    }

    public bool CanBeResolved()
    {
      if (Targets.Effect.None())
        return true;

      if (AllTargetsMustBeValid == false)
      {
        return Targets.Effect.Any(IsValid);
      }

      return Targets.Effect.All(IsValid) &&
        Source.ValidateTargetDependencies(Targets.Cost, Targets.Effect);
    }

    public override string ToString()
    {
      return Source.ToString();
    }

    public void BeginResolve()
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

    public bool HasTag(EffectTag effectTag)
    {
      return _tags.Contains(effectTag);
    }

    public bool HasEffectTarget(Card card)
    {
      return Targets.Effect.Contains(card);
    }

    public virtual Effect Initialize(EffectParameters p, Game game, bool evaluateDynamicParameters = true)
    {
      Game = game;
      Source = p.Source;
      Targets = p.Targets ?? new Targets();
      _triggerMessage = p.TriggerMessage;
      X = p.X;
      Id = game.Recorder.CreateId(this);

      _wasResolved.Initialize(game.ChangeTracker);

      foreach (var parameter in _dynamicParameters)
      {
        parameter.Initialize(ChangeTracker);
      }

      if (evaluateDynamicParameters)
      {
        foreach (var parameter in _dynamicParameters)
        {
          parameter.EvaluateOnInit(this, Game);
        }
      }

      Initialize();
      return this;
    }

    public virtual void SetTriggeredAbilityTargets(Targets targets)
    {
      Targets = targets;

      foreach (var parameter in _dynamicParameters)
      {
        parameter.EvaluateAfterTriggeredAbilityTargets(this, Game);
      }
    }
  }
}