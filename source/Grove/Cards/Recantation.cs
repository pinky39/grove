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

  public class Recantation : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Recantation")
        .ManaCost("{3}{U}{U}")
        .Type("Enchantment")
        .Text(
          "At the beginning of your upkeep, you may put a verse counter on Recantation.{EOL}{U}, Sacrifice Recantation: Return up to X target permanents to their owners' hands, where X is the number of verse counters on Recantation.")
        .Cast(p => p.TimingRule(new SecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, you may put a verse counter on Recantation.";
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect = () => new ApplyModifiersToSelf(() => new AddCounters(() => new ChargeCounter(), 1));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "{U}, Sacrifice Recantation: Return up to X target permanents to their owners' hands, where X is the number of verse counters on Recantation.";

            p.Cost = new AggregateCost(
              new PayMana(Mana.Blue, ManaUsage.Abilities),
              new Sacrifice());

            p.Effect = () => new Gameplay.Effects.ReturnToHand();

            p.TargetSelector.AddEffect(trg =>
              {
                trg.Is.Card().On.Battlefield();
                trg.MinCount = 0;
                trg.GetMaxCount = cp => cp.OwningCard.CountersCount;
              });

            p.TimingRule(new ChargeCounters(3, onlyAtEot: false));
            p.TargetingRule(new Bounce());
            p.TimingRule(new TargetRemoval());
          });
    }
  }
}