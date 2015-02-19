namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;
  using Grove.Triggers;
  using ReturnToHand = Effects.ReturnToHand;

  public class Recantation : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Recantation")
        .ManaCost("{3}{U}{U}")
        .Type("Enchantment")
        .Text(
          "At the beginning of your upkeep, you may put a verse counter on Recantation.{EOL}{U}, Sacrifice Recantation: Return up to X target permanents to their owners' hands, where X is the number of verse counters on Recantation.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, you may put a verse counter on Recantation.";
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect =
              () => new ApplyModifiersToSelf(() => new AddCounters(() => new SimpleCounter(CounterType.Verse), 1));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "{U}, Sacrifice Recantation: Return up to X target permanents to their owners' hands, where X is the number of verse counters on Recantation.";

            p.Cost = new AggregateCost(
              new PayMana(Mana.Blue),
              new Sacrifice());

            p.Effect = () => new ReturnToHand();

            p.TargetSelector.AddEffect(trg =>
              {
                trg.Is.Card().On.Battlefield();
                trg.MinCount = 0;
                trg.GetMaxCount = cp => cp.OwningCard.CountersCount();
              });

            p.TimingRule(new WhenCardHasCounters(3, onlyAtEot: false));
            p.TargetingRule(new EffectBounce());
            p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.Bounce));
          });
    }
  }
}