namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Effects;
  using Core.Details.Mana;
  using Core.Dsl;
  using Core.Targeting;

  public class CarrionBeetles : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Carrion Beetles")
        .ManaCost("{B}")
        .Type("Creature Insect")
        .Text("{2}{B},{T}: Exile up to three target cards from a single graveyard.")
        .FlavorText("It's all fun and games until someone loses an eye.")
        .Power(1)
        .Toughness(1)
        .Timing(Timings.Creatures())
        .Abilities(
          C.ActivatedAbility(
            "{2}{B},{T}: Exile up to three target cards from a single graveyard.",
            C.Cost<TapOwnerPayMana>(cost =>
              {
                cost.TapOwner = true;
                cost.Amount = "{2}{B}".ParseManaAmount();
              }),
            C.Effect<ExileTargets>(),
            effectValidator: C.Validator(Validators.CardInGraveyard(yourGraveyardOnly: false),
              minCount: 0,
              maxCount: 3),
            targetSelectorAi: TargetSelectorAi.RemoveCardsFromOpponentsGraveyard(),
            timing: Timings.EndOfTurn())
        );
    }
  }
}