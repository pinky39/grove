namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;
  using Triggers;

  public class VileRequiem : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Vile Requiem")
        .ManaCost("{2}{B}{B}")
        .Type("Enchantment")
        .Text(
          "At the beginning of your upkeep, you may put a verse counter on Vile Requiem.{EOL}{1}{B}, Sacrifice Vile Requiem: Destroy up to X target nonblack creatures, where X is the number of verse counters on Vile Requiem. They can't be regenerated.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, you may put a verse counter on Vile Requiem.";
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect =
              () => new ApplyModifiersToSelf(() => new AddCounters(() => new SimpleCounter(CounterType.Verse), 1));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "{1}{B}, Sacrifice Vile Requiem: Destroy up to X target nonblack creatures, where X is the number of verse counters on Vile Requiem. They can't be regenerated.";

            p.Cost = new AggregateCost(
              new PayMana("{1}{B}".Parse()),
              new Sacrifice());

            p.Effect = () => new DestroyTargetPermanents(canRegenerate: false);

            p.TargetSelector.AddEffect(
              trg => trg.Is.Card(c => c.Is().Creature && !c.HasColor(CardColor.Black)).On.Battlefield(),
              trg =>
                {
                  trg.MinCount = 0;
                  trg.GetMaxCount = cp => cp.OwningCard.CountersCount();
                });

            p.TimingRule(new WhenCardHasCounters(1, onlyAtEot: false));
            p.TargetingRule(new EffectDestroy());
            p.TimingRule(new TargetRemovalTimingRule().RemovalTags(EffectTag.Destroy, EffectTag.CreaturesOnly));
          });
    }
  }
}