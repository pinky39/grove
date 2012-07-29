namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Dsl;

  public class WildFire : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Wildfire")
        .ManaCost("{4}{R}{R}")
        .Type("Sorcery")
        .Text("Each player sacrifices four lands. Wildfire deals 4 damage to each creature.")
        .FlavorText("'Shiv hatched from a shell of stone around a yolk of flame.'—Viashino myth")        
        .Timing(Timings.NoRestrictions())
        .Effect<CompoundEffect>(p =>
          p.Effect.ChildEffects(
            p.Builder.Effect<PlayersSacrificeLands>(e => e.Count = 4),
            p.Builder.Effect<DealDamageToEach>(e => e.AmountCreature = 4))
        );
    }
  }
}