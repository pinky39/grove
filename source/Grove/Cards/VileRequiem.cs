namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Characteristics;
  using Gameplay.Costs;
  using Gameplay.Counters;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.Modifiers;
  using Gameplay.States;
  using Gameplay.Triggers;

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
              new PayMana("{1}{B}".Parse(), ManaUsage.Abilities),
              new Sacrifice());

            p.Effect = () => new DestroyTargetPermanents(canRegenerate: false);

            p.TargetSelector.AddEffect(trg =>
              {
                trg.Is.Card(c => c.Is().Creature && !c.HasColor(CardColor.Black)).On.Battlefield();
                trg.MinCount = 0;
                trg.GetMaxCount = cp => cp.OwningCard.CountersCount();
              });

            p.TimingRule(new WhenCardHasCounters(1, onlyAtEot: false));                        
            p.TargetingRule(new EffectDestroy());
            p.TimingRule(new TargetRemovalTimingRule());
          });
    }
  }
}