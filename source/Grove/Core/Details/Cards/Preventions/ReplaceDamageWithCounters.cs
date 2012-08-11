namespace Grove.Core.Details.Cards.Preventions
{
  using Counters;
  using Modifiers;
  using Targeting;

  public class ReplaceDamageWithCounters : DamagePrevention
  {
    public ICounterFactory CounterFactory { get; set; }

    public override void PreventReceivedDamage(Damage damage)
    {
      var modifier = Builder
        .Modifier<AddCounters>(m =>
          {
            m.Counter = CounterFactory;
            m.Count = damage.Amount;
          })
          .CreateModifier(Owner.Card(), Owner, x: null);

      Owner.AddModifier(modifier);
      damage.PreventAll();
    }

    public override int EvaluateReceivedDamage(Card source, int amount, bool isCombat)
    {
      return 0;
    }
  }
}