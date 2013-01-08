namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
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
        .Cast(p =>
          {
            p.Timing = Timings.InstantRemovalTarget();
            p.Category = EffectCategories.Destruction;
            p.Effect = Effect<DestroyTargetPermanents>();
            p.EffectTargets = L(Target(Validators.Card(card => card.Is().Creature && !card.Is().Artifact),
              Zones.Battlefield()));
            p.TargetSelectorAi = TargetSelectorAi.Destroy();
          });
    }
  }
}