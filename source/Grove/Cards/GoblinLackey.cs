namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Effects;
  using Core.Triggers;
  using Core.Zones;

  public class GoblinLackey : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Goblin Lackey")
        .ManaCost("{R}")
        .Type("Creature Goblin")
        .Text(
          "Whenever Goblin Lackey successfully deals damage to a player, you may choose a Goblin card in your hand and put that Goblin into play.")
        .FlavorText("All bark, someone else's bite.")
        .Power(1)
        .Toughness(1)        
        .Abilities(
          TriggeredAbility(
            "Whenever Goblin Lackey successfully deals damage to a player, you may choose a Goblin card in your hand and put that Goblin into play.",
            Trigger<OnDamageDealt>(t => t.ToPlayer()),
            Effect<PutSelectedCardToBattlefield>(e =>
              {
                e.Validator = card => card.Is("goblin");
                e.Zone = Zone.Hand;
                e.Text = "Select a goblin in your hand";
              })
            )
        );
    }
  }
}