namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class GoForTheThroat : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Go for the Throat")
        .ManaCost("{1}{B}")
        .Type("Instant")
        .Text("Destroy target nonartifact creature.")
        .FlavorText("Having flesh is increasingly a liability on Mirrodin.")
        .Effect<DestroyTargetPermanents>()
        .Timing(Timings.InstantRemovalTarget())
        .Category(EffectCategories.Destruction)
        .Targets(
          selectorAi: TargetSelectorAi.Destroy(),
          effectValidator: Validator(Validators.Creature(card => !card.Is().Artifact)));
    }
  }
}