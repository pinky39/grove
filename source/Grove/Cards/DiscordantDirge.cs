namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Counters;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Details.Cards.Triggers;
  using Core.Details.Mana;
  using Core.Dsl;

  public class DiscordantDirge : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Discordant Dirge")
        .ManaCost("{3}{B}{B}")
        .Type("Enchantment")
        .Text(
          "At the beginning of your upkeep, you may put a verse counter on Discordant Dirge.{EOL}{B}, Sacrifice Discordant Dirge: Look at target opponent's hand and choose up to X cards from it, where X is the number of verse counters on Discordant Dirge. That player discards those cards.")
        .Timing(Timings.SecondMain())
        .Abilities(
          C.TriggeredAbility(
            "At the beginning of your upkeep, you may put a verse counter on Discordant Dirge.",
            C.Trigger<AtBegginingOfStep>((t, _) => t.Step = Step.Upkeep),
            C.Effect<ApplyModifiersToSelf>(p => p.Effect.Modifiers(
              p.Builder.Modifier<AddCounters>((m, c0) => { m.Counter = c0.Counter<ChargeCounter>(); }))),
            triggerOnlyIfOwningCardIsInPlay: true
            ),
          C.ActivatedAbility(
            "{B}, Sacrifice Discordant Dirge: Look at target opponent's hand and choose up to X cards from it, where X is the number of verse counters on Discordant Dirge. That player discards those cards.",
            C.Cost<SacOwnerPayMana>((cost, _) => cost.Amount = ManaAmount.Black),
            C.Effect<OpponentDiscardsCards>(e =>
              {
                e.SelectedCount = e.Source.OwningCard.Counters.GetValueOrDefault();
                e.YouChooseDiscardedCards = true;
              }),
            timing: All(Timings.Has3CountersOr1IfDestroyed(), Timings.OpponentHasCardsInHand(3))));
    }
  }
}