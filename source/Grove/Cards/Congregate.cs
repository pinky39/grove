namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class Congregate : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Congregate")
        .ManaCost("{3}{W}")
        .Type("Instant")
        .Text("Target player gains 2 life for each creature on the battlefield.")
        .FlavorText(
          "'In the gathering there is strength for all who founder, renewal for all who languish, love for all who sing.'{EOL}—Song of All, canto 642")
        .Timing(All(Timings.EndOfTurn(), Timings.HasPermanents(3, card => card.Is().Creature)))
        .Effect<TargetPlayerGainsLifeEqualToCreatureCount>(e => e.Multiplier = 2)
        .Targets(
          effectValidator: Validator(Validators.Player()),
          selectorAi: TargetSelectorAi.Controller()
        );
    }
  }
}