namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Counters;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Mana;

  public class DiscordantDirge : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Discordant Dirge")
        .ManaCost("{3}{B}{B}")
        .Type("Enchantment")
        .Text(
          "At the beginning of your upkeep, you may put a verse counter on Discordant Dirge.{EOL}{B}, Sacrifice Discordant Dirge: Look at target opponent's hand and choose up to X cards from it, where X is the number of verse counters on Discordant Dirge. That player discards those cards.")
        .Cast(p => p.Timing = Timings.SecondMain())                
        .Abilities(
          TriggeredAbility(
            "At the beginning of your upkeep, you may put a verse counter on Discordant Dirge.",
            Trigger<OnStepStart>(t => t.Step = Step.Upkeep),
            Effect<ApplyModifiersToSelf>(e => e.Modifiers(
              Modifier<AddCounters>(m => { m.Counter = Counter<ChargeCounter>(); }))),
            triggerOnlyIfOwningCardIsInPlay: true
            ),
          ActivatedAbility(
            "{B}, Sacrifice Discordant Dirge: Look at target opponent's hand and choose up to X cards from it, where X is the number of verse counters on Discordant Dirge. That player discards those cards.",
            Cost<PayMana, Sacrifice>(cost => cost.Amount = ManaAmount.Black),
            Effect<OpponentDiscardsCards>(e =>
              {
                e.SelectedCount = e.Source.OwningCard.Counters.GetValueOrDefault();
                e.YouChooseDiscardedCards = true;
              }),
            timing: All(Timings.Has3CountersOr1IfDestroyed(), Timings.OpponentHasCardsInHand(3))));
    }
  }
}