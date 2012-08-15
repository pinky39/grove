namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class Victimize : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Victimize")
        .ManaCost("{2}{B}")
        .Type("Sorcery")
        .Text(
          "Choose two target creature cards in your graveyard. Sacrifice a creature. If you do, return the chosen cards to the battlefield tapped.")
        .FlavorText("The priest cast Xantcha to the ground. 'It is defective. We must scrap it.'")
        .Timing(Timings.SecondMain())
        .Effect<PutTargetsToBattlefield>(e =>
          {
            e.MustSacCreatureOnResolve = true;
            e.Tapped = true;
          })
        .Targets(
          selectorAi: TargetSelectorAi.OrderByDescendingScore(Controller.SpellOwner),
          effectValidator: C.Validator(
            Validators.CardInGraveyard(card => card.Is().Creature),
            minCount: 2,
            maxCount: 2));
    }
  }
}