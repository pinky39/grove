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
    // Holds a list of effect parameters which must be evaluated either 
    // when effect is created, put on stack or resolved.
    private readonly List<IDynamicParameter> _dynamicParameters = new List<IDynamicParameter>();

    // A list of tags to help AI determine what kind of effect is this
    // and what to do in response.
    private readonly List<EffectTag> _tags = new List<EffectTag>();
    private readonly Trackable<bool> _wasResolved = new Trackable<bool>();
    private Value _toughnessReduction = 0;
    private object _triggerMessage;

    // Allows extending effect on the fly
    public Action<Effect> AfterResolve = delegate { };
    public Action<Effect> BeforeResolve = delegate { };

    // If there are multiple targets and this is true all the targets
    // must still be valid for effect to resolve
    protected bool AllTargetsMustBeValidForEffectToResolve = false;

    public bool CanBeCountered = true;

    // Allow custom checks if effect should resolve.
    // Runs right before the Effect would resolve.
    public Func<Effect, bool> ShouldResolve = delegate { return true; };

    // If multiple effect would be put on stack AI uses this to sort them.
    public int TriggerOrderRule = TriggerOrder.Normal;


    // AI uses this to evaluate if it should play some
    // spells in response.
    public Value ToughnessReduction
    {
      get { return _toughnessReduction; }
      set
      {
        _toughnessReduction = value;
        SetTags(EffectTag.ReduceToughness);
      }
    }

    public Player Controller
    {
      get { return Source.OwningCard.Controller; }
    }

    // Effect source can be Activated Ability, Triggered Ability or
    // Casting Rule.
    public IEffectSource Source { get; private set; }

    // If a spell has X, this is the chosen X value.
    public int? X { get; private set; }

    private bool WasResolved
    {
      get { return _wasResolved.Value; }
      set { _wasResolved.Value = value; }
    }

    public Targets Targets { get; private set; }

    public bool IsOnStack
    {
      get { return Stack.Contains(this); }
    }

    // AI uses this to evaluate if it should play some
    // spells in response.
    public virtual bool AffectsEffectSource
    {
      get { return false; }
    }

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

    public CardColor[] Colors
    {
      get { return Source.OwningCard.Colors; }
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

    // If a Triggered ability has created this effect
    // this holds the trigger context.
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

    // After all decisions created by this effect are run
    // this is called to finish resolving the effect.
    public virtual void FinishResolve()
    {
      if (WasResolved)
      {
        AfterResolve(this);
        Source.EffectResolved();
        Publish(new EffectResolvedEvent(this));
        return;
      }

      EffectCountered(SpellCounterReason.IllegalTarget);
    }

    // Before effect is resolved a last check is made if
    // it can be resolved.
    public bool CanBeResolved()
    {
      if (Targets.Effect.Count == 0)
        return true;

      if (AllTargetsMustBeValidForEffectToResolve == false)
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

    // This is called when the effect is removed from stack
    // and starts resolving.
    public virtual void BeginResolve()
    {
      BeforeResolve(this);

      if (ShouldResolve(this))
      {
        foreach (var parameter in _dynamicParameters)
        {
          parameter.EvaluateOnResolve(this, Game);
        }

        ResolveEffect();
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