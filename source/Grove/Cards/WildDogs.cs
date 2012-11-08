namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Triggers;
  using Core.Dsl;

  public class WildDogs : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Wild Dogs")
        .ManaCost("{G}")
        .Type("Creature Hound")
        .Text(
          "At the beginning of your upkeep, if a player has more life than each other player, the player with the most life gains control of Wild Dogs.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .Power(2)
        .Toughness(1)
        .Timing(Timings.Creatures())
        .Cycling("{2}")
        .Abilities(
          TriggeredAbility(
            "At the beginning of your upkeep, if a player has more life than each other player, the player with the most life gains control of Wild Dogs.",
            Trigger<AtBegginingOfStep>(t =>
              {
                t.Step = Step.Upkeep;
                t.Condition = self =>
                  {
                    var controller = self.OwningCard.Controller;
                    var opponent = self.Game.Players.GetOpponent(controller);

                    return controller.Life < opponent.Life;
                  };
              }),
            Effect<SwitchController>(), triggerOnlyIfOwningCardIsInPlay: true));
    }
  }
}