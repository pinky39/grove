namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.Triggers;

  public class MultanisPresence : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Multani's Presence")
        .ManaCost("{G}")
        .Type("Enchantment")
        .Text("Whenever a spell you've cast is countered, draw a card.")
        .FlavorText("When a tree falls in the forest, Multani hears it.")
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever a spell you've cast is countered, draw a card.";
            p.Trigger(new OnCounteredSpell((ability, card) =>
              card.Controller == ability.OwningCard.Controller
              ));

            p.Effect = () => new DrawCards(1);
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}