namespace Grove.Core.Details.Cards.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Ai;
  using Controllers;
  using Dsl;
  using Infrastructure;
  using Targeting;

  [Copyable]
  public abstract class Effect
  {
    public Action<Effect> AfterResolve = delegate { };
    public Action<Effect> BeforeResolve = delegate { };
    private TrackableList<Target> _costTargets;
    private TrackableList<Target> _targets;
    private Trackable<bool> _wasResolved;
    public bool CanBeCountered { get; set; }
    public IPlayer Controller { get { return Source.OwningCard.Controller; } }
    protected Decisions Decisions { get { return Game.Decisions; } }
    protected Game Game { get; set; }
    public Players Players { get { return Game.Players; } }
    public IEffectSource Source { get; set; }
    public int? X { get; set; }
    public virtual bool AffectsSource { get { return false; } }
    public bool HasTargets { get { return _targets.Count > 0; } }
    private bool WasResolved { get { return _wasResolved.Value; } set { _wasResolved.Value = value; } }

    protected IEnumerable<Target> Targets { get { return _targets; } }

    public IEnumerable<Target> AllTargets
    {
      get
      {
        foreach (var costTarget in _costTargets)
        {
          yield return costTarget;
        }

        foreach (var target in _targets)
        {
          yield return target;
        }
      }
    }

    public bool WasKickerPaid { get; set; }

    public int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        GetType().GetHashCode(),
        calc.Calculate(Source),
        calc.Calculate(_targets),
        calc.Calculate(_costTargets),
        CanBeCountered.GetHashCode(),
        WasKickerPaid.GetHashCode(),
        X.GetHashCode());
    }

    public virtual int CalculatePlayerDamage(IPlayer player)
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

    public Target Target()
    {
      return _targets.Count == 0 ? null : _targets[0];
    }

    public Target CostTarget()
    {
      return _costTargets.Count == 0 ? null : _costTargets[0];
    }

    public Target Target(int index)
    {
      return _targets[index];
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
      return _targets.Count == 0 || Source.AreTargetsStillValid(_targets, WasKickerPaid);
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

    public void AddTargets(IEnumerable<Target> targets)
    {
      _targets.AddRange(targets);
    }

    public void AddTarget(Target target)
    {
      _targets.Add(target);
    }

    public void AddCostTargets(IEnumerable<Target> targets)
    {
      _costTargets.AddRange(targets);
    }

    public bool HasTarget(Target card)
    {
      return _targets.Any(x => x == card);
    }

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
            _targets = new TrackableList<Target>(parameters.Targets, Game.ChangeTracker, orderImpactsHashcode: true),
            _costTargets =
              new TrackableList<Target>(parameters.CostTargets, Game.ChangeTracker, orderImpactsHashcode: true),
            _wasResolved = new Trackable<bool>(Game.ChangeTracker),
            WasKickerPaid = parameters.Activation.PayKicker,
            X = parameters.Activation.X,
            CanBeCountered = true
          };

        Init(new EffectCreationContext<TEffect>(
          effect,
          parameters,
          new CardBuilder(Game)));

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