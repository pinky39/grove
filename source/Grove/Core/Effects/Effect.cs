namespace Grove.Core.Effects
{
  using Controllers;
  using Infrastructure;

  [Copyable]
  public abstract class Effect : ITarget
  {
    private bool _wasKickerPaid;
    public bool CanBeCountered { get; set; }
    public Player Controller { get { return Source.OwningCard.Controller; } }
    protected Decisions Decisions { get { return Game.Decisions; } }
    protected Game Game { get; set; }
    public bool HasTarget { get { return Target != null; } }
    protected Players Players { get { return Game.Players; } }
    public IEffectSource Source { get; set; }
    public ITarget Target { get; set; }
    protected int? X { get; private set; }

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

    public abstract void Resolve();

    [Copyable]
    public class Factory<TEffect> : IEffectFactory, IHashable where TEffect : Effect, new()
    {
      public Initializer<TEffect> Init = delegate { };
      public Game Game { get; set; }

      public Effect CreateEffect(IEffectSource source, int? x, bool wasKickerPaid)
      {
        var effect = new TEffect{
          Game = Game,
          Source = source,
          X = x,
          _wasKickerPaid = wasKickerPaid,
          CanBeCountered = true
        };

        Init(effect, new Creator(Game));

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