namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Mana;
  using Core.Targeting;

  public class DoomBlade : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Doom blade")
        .ManaCost("{1}{B}")
        .Type("Instant")
        .Text("Destroy target nonblack creature.")
        .FlavorText("The void is without substance but cuts like steel.")
        .Effect<DestroyTargetPermanents>()
        .Timing(Timings.InstantRemovalTarget())
        .Category(EffectCategories.Destruction)
        .Targets(
          TargetSelectorAi.Destroy(), 
          TargetValidator(
            TargetIs.Card(card => card.Is().Creature && !card.HasColors(ManaColors.Black)),
            ZoneIs.Battlefield()));
    }
  }
}