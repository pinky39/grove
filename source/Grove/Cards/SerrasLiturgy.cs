namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Counters;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Mana;
  using Gameplay.Modifiers;
  using Gameplay.States;

  public class SerrasLiturgy : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Serra's Liturgy")
        .ManaCost("{2}{W}{W}")
        .Type("Enchantment")
        .Text(
          "At the beginning of your upkeep, you may put a verse counter on Serra's Liturgy.{EOL}{W}, Sacrifice Serra's Liturgy: Destroy up to X target artifacts and/or enchantments, where X is the number of verse counters on Serra's Liturgy.")
        .Cast(p => p.TimingRule(new SecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, you may put a verse counter on Serra's Liturgy.";
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect = () => new ApplyModifiersToSelf(() => new AddCounters(() => new ChargeCounter(), 1));
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
                trg.GetMaxCount = cp => cp.OwningCard.CountersCount;
              });

            p.TimingRule(new ChargeCounters(3, onlyAtEot: false));
            p.TimingRule(new Steps(activeTurn: true, passiveTurn: false, steps: Step.FirstMain));
            p.TargetingRule(new Destroy());
          });
    }
  }
}