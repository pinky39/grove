namespace Grove.Core.Details.Cards.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Ai;
  using Controllers;
  using Dsl;
  using Infrastructure;
  using Mana;
  using Targeting;

  [Copyable]
  public abstract class Effect : ITarget, IHasColors
  {
    public Action<Effect> AfterResolve = delegate { };
    public Action<Effect> BeforeResolve = delegate { };
    private Targets _targets;
    private Trackable<bool> _wasResolved;
    public bool CanBeCountered { get; set; }
    public Player Controller { get { return Source.OwningCard.Controller; } }
    protected Decisions Decisions { get { return Game.Decisions; } }
    protected Game Game { get; set; }
    public Players Players { get { return Game.Players; } }
    public IEffectSource Source { get; set; }
    public int? X { get; set; }
    public virtual bool AffectsSource { get { return false; } }
    public bool HasTargets { get { return _targets.Count > 0; } }
    private bool WasResolved { get { return _wasResolved.Value; } set { _wasResolved.Value = value; } }

    // ui uses this to display targets of an effect
    public object UiTargets { get { return _targets; } }

    public bool WasKickerPaid { get; set; }
    protected IList<ITarget> Targets { get { return _targets.Effect; } }

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
        WasKickerPaid.GetHashCode(),
        X.GetHashCode());
    }

    public virtual int CalculatePlayerDamage(Player player)
    {
      return 0;
    }

    public virtual int CalculateCreatureDamage(Card creature)
    {
      return 0;
    }

    public virtual int CalculateToughnessReduction(Card creature)
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

    public void EffectWasCountered()
    {
      Source.EffectWasCountered();
    }

    public void EffectWasPushedOnStack()
    {
      Source.EffectWasPushedOnStack();
    }

    public void FinishResolve()
    {
      if (WasResolved)
      {
        Source.EffectWasResolved();
        return;
      }

      Source.EffectWasCountered();
    }

    public bool HasEffectStillValidTargets()
    {
      return _targets.Count == 0 || Source.AreTargetsStillValid(_targets.Effect, WasKickerPaid);
    }

    public override string ToString()
    {
      return Source.ToString();
    }

    public void Resolve()
    {
      BeforeResolve(this);
      ResolveEffect();
      AfterResolve(this);
      WasResolved = true;
    }

    protected abstract void ResolveEffect();

    public bool HasCategory(EffectCategories effectCategories)
    {
      return ((effectCategories & Source.EffectCategories) != EffectCategories.Generic);
    }

    public bool HasTarget(Card card)
    {
      return _targets.Any(x => x == card);
    }

    protected virtual void Init() {}

    [Copyable]
    public class Factory<TEffect> : IEffectFactory where TEffect : Effect, new()
    {
      public EffectInitializer<TEffect> Init = delegate { };
      public Game Game { get; set; }

      public Effect CreateEffect(EffectParameters parameters)
      {
        var effect = new TEffect
          {
            Game = Game,
            Source = parameters.Source,
            _wasResolved = new Trackable<bool>(Game.ChangeTracker),
            _targets = parameters.Targets,
            WasKickerPaid = parameters.Activation.PayKicker,
            X = parameters.Activation.X,
            CanBeCountered = true
          };

        Init(new EffectCreationContext<TEffect>(
          effect,
          parameters,
          new CardBuilder(Game)));

        effect.Init();

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