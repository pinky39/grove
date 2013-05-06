namespace Grove.Gameplay.Damage
{
  using System;
  using Counters;
  using Modifiers;
  using Targeting;

  public class ReplaceDamageWithCounters : DamagePrevention
  {
    private readonly Func<Counter> _counter;

    private ReplaceDamageWithCounters() {}

    public ReplaceDamageWithCounters(Func<Counter> counter)
    {
      _counter = counter;
    }

    public override void PreventReceivedDamage(Damage damage)
    {
      var mp = new ModifierParameters
        {
          SourceCard = Owner.Card(),
          Target = Owner,
        };

      var modifier = new AddCounters(_counter, damage.Amount);
      modifier.Initialize(mp, Game);

      Owner.AddModifier(modifier);
      damage.PreventAll();
    }

    public override int EvaluateReceivedDamage(Card source, int amount, bool isCombat)
    {
      return 0;
    }
  }
}