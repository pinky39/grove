namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Targeting;

  public class CarrionBeetles : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Carrion Beetles")
        .ManaCost("{B}")
        .Type("Creature Insect")
        .Text("{2}{B},{T}: Exile up to three target cards from a single graveyard.")
        .FlavorText("It's all fun and games until someone loses an eye.")
        .Power(1)
        .Toughness(1)        
        .Abilities(
          ActivatedAbility(
            "{2}{B},{T}: Exile up to three target cards from a single graveyard.",
            Cost<PayMana, Tap>(cost => cost.Amount = "{2}{B}".ParseMana()),
            Effect<ExileTargets>(),
            effectTarget: Target(
              Validators.Card(),
              Zones.Graveyard(),
              minCount: 0,
              maxCount: 3),
            targetingAi: TargetingAi.RemoveCardsFromOpponentsGraveyard(),
            timing: Timings.EndOfTurn())
        );
    }
  }
}