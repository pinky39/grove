namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Costs;
  using Gameplay.Counters;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.Modifiers;
  using Gameplay.States;
  using Gameplay.Triggers;

  public class SerrasLiturgy : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Serra's Liturgy")
        .ManaCost("{2}{W}{W}")
        .Type("Enchantment")
        .Text(
          "At the beginning of your upkeep, you may put a verse counter on Serra's Liturgy.{EOL}{W}, Sacrifice Serra's Liturgy: Destroy up to X target artifacts and/or enchantments, where X is the number of verse counters on Serra's Liturgy.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, you may put a verse counter on Serra's Liturgy.";
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect =
              () => new ApplyModifiersToSelf(() => new AddCounters(() => new SimpleCounter(CounterType.Verse), 1));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "{W}, Sacrifice Serra's Liturgy: Destroy up to X target artifacts and/or enchantments, where X is the number of verse counters on Serra's Liturgy.";

            p.Cost = new AggregateCost(
              new PayMana(Mana.White, ManaUsage.Abilities),
              new Sacrifice());

            p.Effect = () => new DestroyTargetPermanents();

            p.TargetSelector.AddEffect(trg =>
              {
                trg.Is.Card(c => c.Is().Enchantment || c.Is().Artifact).On.Battlefield();
                trg.MinCount = 0;
                trg.GetMaxCount = cp => cp.OwningCard.CountersCount();
              });

            p.TimingRule(new WhenCardHasCounters(3, onlyAtEot: false));
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectDestroy());
          });
    }
  }
}