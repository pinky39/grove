namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TargetingRules;
  using Core.Ai.TimingRules;
  using Core.Costs;
  using Core.Counters;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Modifiers;
  using Core.Triggers;

  public class RumblingCrescendo : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Rumbling Crescendo")
        .ManaCost("{3}{R}{R}")
        .Type("Enchantment")
        .Text("At the beginning of your upkeep, you may put a verse counter on Rumbling Crescendo.{EOL}{R}, Sacrifice Rumbling Crescendo: Destroy up to X target lands, where X is the number of verse counters on Rumbling Crescendo.")
        .Cast(p => p.TimingRule(new SecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, you may put a verse counter on Rumbling Crescendo.";
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect = () => new ApplyModifiersToSelf(() => new AddCounters(() => new ChargeCounter(), 1));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "{R}, Sacrifice Rumbling Crescendo: Destroy up to X target lands, where X is the number of verse counters on Rumbling Crescendo.";

            p.Cost = new AggregateCost(
              new PayMana(Mana.Red, ManaUsage.Abilities),
              new Sacrifice());

            p.Effect = () => new DestroyTargetPermanents();

            p.TargetSelector.AddEffect(trg =>
              {
                trg.Is.Card(c => c.Is().Land).On.Battlefield();
                trg.MinCount = 0;
                trg.GetMaxCount = cp => cp.OwningCard.CountersCount;
              });

            p.TimingRule(new ChargeCounters(3, onlyAtEot: false));
            p.TimingRule(new Steps(activeTurn: true, passiveTurn: false, steps: Step.FirstMain));
            p.TargetingRule(new Destroy());            
          });
    }
  }
}