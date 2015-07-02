namespace Grove.Effects
{
  using System;
  using System.Linq;
  using Modifiers;

  public class PutCounterOnYoursAndFightWithOpponentsCreature : Effect
  {
    private readonly Func<Counter> _counter;
    private readonly Value _count;

    private PutCounterOnYoursAndFightWithOpponentsCreature() {}

    public PutCounterOnYoursAndFightWithOpponentsCreature(Func<Counter> counter, Value count = null)
    {
      _counter = counter;
      _count = count ?? 1;
    }

    protected override void ResolveEffect()
    {
      var p = new ModifierParameters
        {
          SourceEffect = this,
          SourceCard = Source.OwningCard,
          X = X
        };

      var attacker = ValidEffectTargets.ToList()[0].Card();
      var blocker = ValidEffectTargets.ToList()[1].Card();

      attacker.AddModifier(new AddCounters(_counter, _count), p);

      attacker.DealDamageTo(attacker.Power.GetValueOrDefault(), blocker, false);
      blocker.DealDamageTo(blocker.Power.GetValueOrDefault(), attacker, false);
    }
  }
}