namespace Grove.Core.Details.Cards.Preventions
{
  using Counters;
  using Modifiers;
  using Targeting;

  public class ReplaceDamageWithCounters : DamagePrevention
  {
    public ICounterFactory CounterFactory { get; set; }

    public override void PreventDamage(Damage damage)
    {
      var factory = new Modifier.Factory<AddCounters>
        {
          Game = Game,
          Init = (m, _) =>
            {
              m.Counter = CounterFactory;
              m.Count = damage.Amount;
            }
        };


      var modifier = factory.CreateModifier(Owner.Card(), Owner);
      Owner.AddModifier(modifier);


      damage.PreventAll();
    }

    public override int EvaluateHowMuchDamageCanBeDealt(Card source, int amount, bool isCombat)
    {
      return 0;
    }
  }
}