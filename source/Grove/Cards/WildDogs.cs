namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;
  using Core.Effects;
  using Core.Triggers;

  public class WildDogs : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Wild Dogs")
        .ManaCost("{G}")
        .Type("Creature Hound")
        .Text(
          "At the beginning of your upkeep, if a player has more life than each other player, the player with the most life gains control of Wild Dogs.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .Power(2)
        .Toughness(1)
        .Cycling("{2}")
        .TriggeredAbility(p =>
          {
            p.Text =
              "At the beginning of your upkeep, if a player has more life than each other player, the player with the most life gains control of Wild Dogs.";
            p.Trigger(new OnStepStart(Step.Upkeep)
              {
                Condition = (t, g) =>
                  {
                    var controller = t.OwningCard.Controller;
                    var opponent = g.Players.GetOpponent(controller);

                    return controller.Life < opponent.Life;
                  }
              });
            p.Effect = () => new SwitchController();
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}