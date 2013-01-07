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
        .Cast(p =>
          {
            p.Timing = Timings.InstantRemovalTarget();
            p.Category = EffectCategories.Destruction;
            p.Effect = Effect<DestroyTargetPermanents>();
            p.EffectTargets = L(Target(Validators.Card(card => card.Is().Creature && !card.HasColors(ManaColors.Black)),
              Zones.Battlefield()));
            p.TargetSelectorAi = TargetSelectorAi.Destroy();
          });                
    }
  }
}