namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Mana;
  using Core.Dsl;
  using Core.Targeting;

  public class DoomBlade : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Doom blade")
        .ManaCost("{1}{B}")
        .Type("Instant")
        .Text("Destroy target nonblack creature.")
        .FlavorText("The void is without substance but cuts like steel.")
        .Effect<DestroyTargetPermanents>()
        .Timing(Timings.InstantRemovalTarget())
        .Category(EffectCategories.Destruction)
        .Targets(
          selectorAi: TargetSelectorAi.Destroy(),
          effectValidator: C.Validator(Validators.Creature((creature) => !creature.HasColors(ManaColors.Black))));
    }
  }
}