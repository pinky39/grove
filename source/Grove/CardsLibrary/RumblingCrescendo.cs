namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;
  using Grove.Triggers;

  public class RumblingCrescendo : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Rumbling Crescendo")
        .ManaCost("{3}{R}{R}")
        .Type("Enchantment")
        .Text(
          "At the beginning of your upkeep, you may put a verse counter on Rumbling Crescendo.{EOL}{R}, Sacrifice Rumbling Crescendo: Destroy up to X target lands, where X is the number of verse counters on Rumbling Crescendo.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, you may put a verse counter on Rumbling Crescendo.";
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect =
              () => new ApplyModifiersToSelf(() => new AddCounters(() => new SimpleCounter(CounterType.Verse), 1));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "{R}, Sacrifice Rumbling Crescendo: Destroy up to X target lands, where X is the number of verse counters on Rumbling Crescendo.";

            p.Cost = new AggregateCost(
              new PayMana(Mana.Red),
              new Sacrifice());

            p.Effect = () => new DestroyTargetPermanents();

            p.TargetSelector.AddEffect(
              trg => trg.Is.Card(c => c.Is().Land).On.Battlefield(), 
              trg =>
              {                
                trg.MinCount = 0;
                trg.GetMaxCount = cp => cp.OwningCard.CountersCount();
              });

            p.TimingRule(new WhenCardHasCounters(minCount: 3, onlyAtEot: false));
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectDestroy());
          });
    }
  }
}