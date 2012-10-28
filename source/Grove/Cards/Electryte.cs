namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Triggers;
  using Core.Dsl;

  public class Electryte : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Electryte")
        .ManaCost("{3}{R}{R}")
        .Type("Creature - Beast")
        .Text("Whenever Electryte deals combat damage to defending player, it deals damage equal to its power to each blocking creature.")
        .FlavorText("Shivan inhabitants are hardened to fire, so their predators have developed alternative weaponry.")
        .Power(3)
        .Toughness(3)
        .Timing(Timings.Creatures())
        .Abilities(
          C.TriggeredAbility(
            "Whenever Electryte deals combat damage to defending player, it deals damage equal to its power to each blocking creature.",
            C.Trigger<DealDamageToCreatureOrPlayer>(t =>
              {
                t.CombatOnly = true;
                t.ToPlayer();
              }),
            C.Effect<DealDamageToEach>(e =>
              {
                e.FilterCreature = (self, card) => card.IsBlocker;
                e.AmountCreature = e.Source.OwningCard.Power;
              })              
            ));
    }
  }
}