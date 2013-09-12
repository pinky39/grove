namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.CostRules;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Costs;
  using Gameplay.Counters;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.Modifiers;
  using Gameplay.Triggers;
  using Gameplay.Zones;

  public class Anthroplasm : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Anthroplasm")
        .ManaCost("{2}{U}{U}")
        .Type("Creature Shapeshifter")
        .Text(
          "Anthroplasm enters the battlefield with two +1/+1 counters on it.{EOL}{X},{T}: Remove all +1/+1 counters from Anthroplasm and put X +1/+1 counters on it.")
        .FlavorText("I never metamorphosis I didn't like.")
        .Power(0)
        .Toughness(0)
        .TriggeredAbility(p =>
          {
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new ApplyModifiersToSelf(() => new AddCounters(() => new PowerToughness(1, 1), 2));
            p.UsesStack = false;
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{X},{T}: Remove all +1/+1 counters from Anthroplasm and put X +1/+1 counters on it.";

            p.Cost = new AggregateCost(
              new PayMana(Mana.Zero, ManaUsage.Abilities, hasX: true),
              new Tap());

            p.Effect = () => new CompoundEffect(
              new RemoveAllCountersFromOwner(CounterType.PowerToughnes),
              new ApplyModifiersToSelf(() => new AddCounters(() => new PowerToughness(1, 1), Value.PlusX)));

            p.CostRule(new XIsAvailableMana());
            p.TimingRule(new Any(new PumpOwningCardTimingRule(Value.PlusX, Value.PlusX), new OnEndOfOpponentsTurn()));
          });
    }
  }
}