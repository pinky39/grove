namespace Grove.Core.Cards.Preventions
{
  using Counters;
  using Grove.Core.Targeting;
  using Modifiers;

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
          .CreateModifier(Owner.Card(), Owner, null, Game);

      Owner.AddModifier(modifier);
      damage.PreventAll();
    }

    public override int EvaluateReceivedDamage(Card source, int amount, bool isCombat)
    {
      return 0;
    }
  }
}