namespace Grove.Core.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Core.Ai;
  using Grove.Core.Dsl;
  using Grove.Infrastructure;
  using Grove.Core.Mana;
  using Grove.Core.Messages;
  using Grove.Core.Targeting;

  [Copyable]
  public abstract class Effect : ITarget, IHasColors
  {
    public Action<Effect> AfterResolve = delegate { };
    public Func<Effect, bool> BeforeResolve = delegate { return true; };
    private Targets _targets;
    protected EffectCategories EffectCategories { get; set; }
    private Trackable<bool> _wasResolved;
    public bool CanBeCountered { get; set; }
    public Player Controller { get { return Source.OwningCard.Controller; } }
    protected Game Game { get; set; }
    public Players Players { get { return Game.Players; } }
    public IEffectSource Source { get; set; }
    public int? X { get; set; }
    public virtual bool AffectsSource { get { return false; } }
    public bool HasTargets { get { return _targets.Effect.Count > 0; } }
    protected CardBuilder Builder { get { return new CardBuilder(); } }
    private bool WasResolved { get { return _wasResolved.Value; } set { _wasResolved.Value = value; } }

    // this is used by ui to display effect targets
    public object UiTargets { get { return _targets; } }    
    protected IList<ITarget> Targets { get { return _targets.Effect; } }
    protected IEnumerable<ITarget> ValidTargets { get { return Targets.Where(IsValid); } }
    protected IList<ITarget> CostTargets { get { return _targets.Cost; } }

    public bool HasColors(ManaColors colors)
    {
      return Source.OwningCard.HasColors(colors);
    }

    public int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        GetType().GetHashCode(),
        calc.Calculate(Source),
        calc.Calculate(_targets),
        CanBeCountered.GetHashCode(),        
        X.GetHashCode());
    }

    protected string FormatText(string text)
    {
      return String.Format("{0}: {1}", Source.SourceCard, text);
    }

    public bool IsValid(ITarget target)
    {
      return Source.IsTargetStillValid(target);
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

    protected Targets GetAllTargets()
    {
      return _targets;
    }

    public ITarget Target()
    {
      return _targets.Effect.Count == 0 ? null : _targets.Effect[0];
    }

    public ITarget CostTarget()
    {
      return _targets.Cost.Count == 0 ? null : _targets.Cost[0];
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
        
        Game.Publish(new EffectResolved {Effect = this});
        return;
      }

      EffectCountered(SpellCounterReason.IllegalTarget);
    }

    public bool CanBeResolved()
    {
      return _targets.Effect.None() ||
        _targets.Effect.Any(IsValid);
    }

    public override string ToString()
    {
      return Source.ToString();
    }

    public void Resolve()
    {
      if (BeforeResolve(this))
      {
        ResolveEffect();
        AfterResolve(this);
      }
      
      WasResolved = true;      
    }

    protected abstract void ResolveEffect();

    public bool HasCategory(EffectCategories effectCategories)
    {
      return ((effectCategories & EffectCategories) != EffectCategories.Generic);
    }

    public bool HasTarget(Card card)
    {
      return _targets.Any(x => x == card);
    }

    protected virtual void Init() {}

    protected virtual void DistributeDamage(IDamageDistributor damageDistributor) {}

    [Copyable]
    public class Factory<TEffect> : IEffectFactory where TEffect : Effect, new()
    {
      public EffectInitializer<TEffect> Init = delegate { };

      public Effect CreateEffect(EffectParameters parameters, Game game)
      {
        var effect = new TEffect
          {
            Game = game,
            Source = parameters.Source,
            EffectCategories = parameters.EffectCategories,
            _wasResolved = new Trackable<bool>(game.ChangeTracker),
            _targets = parameters.Targets,            
            X = parameters.Activation.X,
            CanBeCountered = true
          };

        Init(new EffectCreationContext<TEffect>(
          effect,
          parameters));

        effect.Init();

        effect.DistributeDamage(parameters.Targets.DamageDistributor);
        return effect;
      }

      public int CalculateHash(HashCalculator calc)
      {
        return HashCalculator.Combine(
          typeof (TEffect).GetHashCode(),
          Init.GetHashCode());
      }
    }
  }
}