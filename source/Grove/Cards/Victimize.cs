namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class Victimize : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Victimize")
        .ManaCost("{2}{B}")
        .Type("Sorcery")
        .Text(
          "Choose two target creature cards in your graveyard. Sacrifice a creature. If you do, return the chosen cards to the battlefield tapped.")
        .FlavorText("The priest cast Xantcha to the ground. 'It is defective. We must scrap it.'")
        .Timing(All(Timings.SecondMain(), Timings.HasPermanent(card => card.Is().Creature)))
        .Effect<PutTargetsToBattlefield>(e =>
          {
            e.MustSacCreatureOnResolve = true;
            e.Tapped = true;
          })
        .Targets(
          TargetSelectorAi.OrderByDescendingScore(Controller.SpellOwner),
          Target(
            Validators.Card(card => card.Is().Creature), Zones.YourGraveyard(),
            minCount: 2,
            maxCount: 2));
    }
  }
}