namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Counters;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Mana;
  using Gameplay.Modifiers;
  using Gameplay.States;

  public class DiscordantDirge : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Discordant Dirge")
        .ManaCost("{3}{B}{B}")
        .Type("Enchantment")
        .Text(
          "At the beginning of your upkeep, you may put a verse counter on Discordant Dirge.{EOL}{B}, Sacrifice Discordant Dirge: Look at target opponent's hand and choose up to X cards from it, where X is the number of verse counters on Discordant Dirge. That player discards those cards.")
        .Cast(p => p.TimingRule(new SecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, you may put a verse counter on Discordant Dirge.";
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect = () => new ApplyModifiersToSelf(() => new AddCounters(() => new ChargeCounter(), 1));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "{B}, Sacrifice Discordant Dirge: Look at target opponent's hand and choose up to X cards from it, where X is the number of verse counters on Discordant Dirge. That player discards those cards.";

            p.Cost = new AggregateCost(
              new PayMana(Mana.Black, ManaUsage.Abilities),
              new Sacrifice());

            p.Effect = () => new OpponentDiscardsCards(
              selectedCount: P(e => e.Source.OwningCard.Counters.GetValueOrDefault()),
              youChooseDiscardedCards: true);

            p.TimingRule(new ChargeCounters(3));
          });
    }
  }
}