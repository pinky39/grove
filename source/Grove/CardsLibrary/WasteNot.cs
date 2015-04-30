namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class WasteNot : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Waste Not")
        .ManaCost("{1}{B}")
        .Type("Enchantment")
        .Text(
          "Whenever an opponent discards a creature card, put a 2/2 black Zombie creature token onto the battlefield.{EOL}Whenever an opponent discards a land card, add {B}{B} to your mana pool.{EOL}Whenever an opponent discards a noncreature, nonland card, draw a card.")
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever an opponent discards a creature card, put a 2/2 black Zombie creature token onto the battlefield.";

            p.Trigger(new WhenPlayerDiscardsCard((e, ctx) =>
              ctx.Opponent == e.Player && e.Card.Is().Creature));

            p.Effect = () => new CreateTokens(count: 1,
              token: Card
                .Named("Zombie")
                .Power(2)
                .Toughness(2)
                .Type("Token Creature - Zombie")
                .Colors(CardColor.Black));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever an opponent discards a land card, add {B}{B} to your mana pool.";

            p.Trigger(new WhenPlayerDiscardsCard((e, ctx) =>
              ctx.Opponent == e.Player && e.Card.Is().Land));

            p.Effect = () => new AddManaToPool("{B}{B}".Parse());

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever an opponent discards a noncreature, nonland card, draw a card.";

            p.Trigger(new WhenPlayerDiscardsCard((e, ctx) =>
              ctx.Opponent == e.Player && !e.Card.Is().Land && !e.Card.Is().Creature));

            p.Effect = () => new DrawCards(1);

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}