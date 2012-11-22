namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Cards.Triggers;
  using Core.Dsl;

  public class Electryte : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Electryte")
        .ManaCost("{3}{R}{R}")
        .Type("Creature - Beast")
        .Text("Whenever Electryte deals combat damage to defending player, it deals damage equal to its power to each blocking creature.")
        .FlavorText("Shivan inhabitants are hardened to fire, so their predators have developed alternative weaponry.")
        .Power(3)
        .Toughness(3)
        .Timing(Timings.Creatures())
        .Abilities(
          TriggeredAbility(
            "Whenever Electryte deals combat damage to defending player, it deals damage equal to its power to each blocking creature.",
            Trigger<DealDamageToCreatureOrPlayer>(t =>
              {
                t.CombatOnly = true;
                t.ToPlayer();
              }),
            Effect<DealDamageToEach>(e =>
              {
                e.FilterCreature = (self, card) => card.IsBlocker;
                e.AmountCreature = e.Source.OwningCard.Power.GetValueOrDefault();
              })              
            ));
    }
  }
}