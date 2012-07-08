namespace Grove.Core.Preventions
{
  using Counters;
  using Modifiers;

  public class ReplaceDamageWithCounters : DamagePrevention
  {
    public ICounterFactory CounterFactory { get; set; }

    public override int PreventDamage(Card damageDealer, int damageAmount, bool queryOnly)
    {
      if (!queryOnly)
      {
        var factory = new Modifier.Factory<AddCounters>
          {
            Game = Game,
            Init = (m, _) =>
              {
                m.Counter = CounterFactory;
                m.Count = damageAmount;
              }
          };


        Modifier modifier = factory.CreateModifier(Target.Card(), Target);
        Target.AddModifier(modifier);
      }

      return 0;
    }
  }
}