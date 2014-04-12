namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Triggers;

  public class PowderKeg : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Powder Keg")
        .ManaCost("{2}")
        .Type("Artifact")
        .Text(
          "At the beginning of your upkeep, you may put a fuse counter on Powder Keg.{EOL}{T}, Sacrifice Powder Keg: Destroy each artifact and creature with converted mana cost equal to the number of fuse counters on Powder Keg.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, you may put a fuse counter on Powder Keg.";
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect = () => new ChooseToAddCounter(CounterType.Fuse,
              e =>
                {
                  var opponent = e.Source.OwningCard.Controller.Opponent;

                  if (opponent.Battlefield.Count == 0)
                    return false;

                  return e.Source.OwningCard.Counters <=
                    opponent.Battlefield.Max(c => c.ConvertedCost);
                });

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "{T}, Sacrifice Powder Keg: Destroy each artifact and creature with converted mana cost equal to the number of fuse counters on Powder Keg.";

            p.Cost = new AggregateCost(
              new Tap(),
              new Sacrifice());

            p.Effect = () => new DestroyAllPermanents(
              filter: (e, count, c) => c.ConvertedCost == count,
              countOnInit: P(e => e.Source.OwningCard.CountersCount(CounterType.Fuse)));

            p.TimingRule(new MassRemovalTimingRule(EffectTag.Destroy));
          });
    }
  }
}