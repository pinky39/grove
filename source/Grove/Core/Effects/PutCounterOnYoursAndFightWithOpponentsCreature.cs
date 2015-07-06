namespace Grove.Effects
{
  using System;
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
      var yours = (Card) Targets.Effect[0];
      var opponents = (Card) Targets.Effect[1];

      if (IsValid(yours))
      {
        var p = new ModifierParameters
          {
            SourceEffect = this,
            SourceCard = Source.OwningCard,
            X = X
          };

        yours.AddModifier(new AddCounters(_counter, _count), p);

        if (IsValid(opponents))
        {
          yours.DealDamageTo(yours.Power.GetValueOrDefault(), opponents, false);
          opponents.DealDamageTo(opponents.Power.GetValueOrDefault(), yours, false);
        }
      }
    }
  }
}