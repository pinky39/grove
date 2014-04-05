namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Modifiers;

  public class Opalescence : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Opalescence")
        .ManaCost("{2}{W}{W}")
        .Type("Enchantment")
        .Text(
          "Each other non-Aura enchantment is a creature with power and toughness each equal to its converted mana cost. It's still an enchantment.")
        .Cast(p =>
          {
            p.TimingRule(new OnFirstMain());
            p.TimingRule(new WhenYouHavePermanents(c => c.Is().Enchantment && !c.Is().Aura));
          })
        .ContinuousEffect(p =>
          {
            p.Modifier = () => new ChangeToCreature(
              power: m => m.OwningCard.ConvertedCost,
              toughness: m => m.OwningCard.ConvertedCost,
              type: m => m.OwningCard.Type + " Creature");

            p.CardFilter = (card, effect) => card != effect.Source && card.Is().Enchantment && !card.Is().Aura;
          });
    }
  }
}