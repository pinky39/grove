namespace Grove.Core.Effects
{
  using System;
  using Ai;
  using CardDsl;
  using Controllers;
  using Infrastructure;

  [Copyable]
  public abstract class Effect : ITarget
  {
    public Action<Effect> AfterResolve = delegate { };
    public Action<Effect> BeforeResolve = delegate { };
    private object _triggerContext;
    private bool _wasKickerPaid;
    public bool CanBeCountered { get; set; }
    public Player Controller { get { return Source.OwningCard.Controller; } }
    protected Decisions Decisions { get { return Game.Decisions; } }
    protected Game Game { get; set; }
    public bool HasTarget { get { return Target != null; } }
    public Players Players { get { return Game.Players; } }
    public IEffectSource Source { get; set; }
    public ITarget Target { get; set; }
    public ITarget DamageSourceTarget { get; set; }
    public int? X { get; private set; }
    public virtual bool AffectsSelf { get { return false; } }

    public int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        GetType().GetHashCode(),
        calc.Calculate(Source),
        calc.Calculate(Target),
        CanBeCountered.GetHashCode(),
        _wasKickerPaid.GetHashCode(),
        X.GetHashCode());
    }

    public T Ctx<T>()
    {
      return (T) _triggerContext;
    }

    public void EffectWasCountered()
    {
      Source.EffectWasCountered();
    }

    public void EffectWasPushedOnStack()
    {
      Source.EffectWasPushedOnStack();
    }

    public void EffectWasResolved()
    {
      Source.EffectWasResolved();
    }

    public bool IsTargetStillValid()
    {
      return Target == null || Source.IsTargetValid(Target);
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
    }

    protected abstract void ResolveEffect();

    public bool HasCategory(EffectCategories effectCategories)
    {
      return ((effectCategories & Source.EffectCategories) != EffectCategories.Generic);
    }

    [Copyable]
    public class Factory<TEffect> : IEffectFactory, IHashable where TEffect : Effect, new()
    {
      public Initializer<TEffect> Init = delegate { };
      public Game Game { get; set; }

      public Effect CreateEffect(IEffectSource source, int? x, bool wasKickerPaid, object triggerContext)
      {
        var effect = new TEffect
          {
            Game = Game,
            Source = source,
            _triggerContext = triggerContext,
            X = x,
            _wasKickerPaid = wasKickerPaid,
            CanBeCountered = true
          };

        Init(effect, new CardCreationCtx(Game));

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