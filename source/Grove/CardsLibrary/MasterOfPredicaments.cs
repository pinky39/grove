namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class MasterOfPredicaments : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Master of Predicaments")
        .ManaCost("{3}{U}{U}")
        .Type("Creature — Sphinx")
        .Text("{Flying}{EOL}Whenever Master of Predicaments deals combat damage to a player, choose a card in your hand. That player guesses whether the card's converted mana cost is greater than 4. If the player guessed wrong, you may cast the card without paying its mana cost.")
        .Power(4)
        .Toughness(4)
        .SimpleAbilities(Static.Flying)
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever Master of Predicaments deals combat damage to a player, choose a card in your hand. That player guesses whether the card's converted mana cost is greater than 4. If the player guessed wrong, you may cast the card without paying its mana cost.";

          p.Trigger(new OnDamageDealt(dmg =>
            dmg.IsDealtByOwningCard &&
              dmg.IsCombat &&
              dmg.IsDealtToPlayer));          
            
          p.Effect = () => new PutSelectedCardIntoPlayIfOpponentGuessedWrong(
            question: "Has opponent selected a card with converted mana cost > 4?",
            chooseAnswer: e => false,
            isCorrectAnswer: (card, answer) => card.ConvertedCost > 4 == answer);
        });
    }
  }
}
