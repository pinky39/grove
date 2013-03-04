namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TimingRules;
  using Core.Costs;
  using Core.Counters;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Modifiers;
  using Core.Triggers;

  public class LotusBlossom : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Lotus Blossom")
        .ManaCost("{2}")
        .Type("Artifact")
        .Text(
          "At the beginning of your upkeep, you may put a petal counter on Lotus Blossom.{EOL}{T}, Sacrifice Lotus Blossom: Add X mana of any color to your mana pool, where X is the number of petal counters on Lotus Blossom.")
        .Cast(p => p.TimingRule(new SecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, you may put a petal counter on Lotus Blossom.";
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect = () => new ApplyModifiersToSelf(() => new AddCounters(() => new ChargeCounter(), 1));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .ManaAbility(p =>
          {
            p.Text =
              "{T}, Sacrifice Lotus Blossom: Add X mana of any color to your mana pool, where X is the number of petal counters on Lotus Blossom.";

            p.Cost = new AggregateCost(
              new Tap(),
              new Sacrifice());

            p.ManaAmount((ability, game) =>
              ManaAmount.OfSingleColor(ManaColors.All, ability.OwningCard.Counters.GetValueOrDefault()));
          });
    }
  }
}