namespace Grove.Effects
{
  using System.Linq;
  using Modifiers;

  public class PutCounterOnTargetAndFightWithCreature : Effect
  {
    private readonly AddCounters _counter;

    private PutCounterOnTargetAndFightWithCreature() {}

    public PutCounterOnTargetAndFightWithCreature(AddCounters counter)
    {
      _counter = counter;
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

      attacker.AddModifier(_counter, p);

      attacker.DealDamageTo(attacker.Power.GetValueOrDefault(), blocker, false);
      blocker.DealDamageTo(blocker.Power.GetValueOrDefault(), attacker, false);
    }
  }
}
