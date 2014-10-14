namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class GarruksPackleader : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Garruk's Packleader")
        .ManaCost("{4}{G}")
        .Type("Creature - Beast")
        .Text("Whenever another creature with power 3 or greater enters the battlefield under your control, you may draw a card.")
        .FlavorText("\"He has learned much in his long years. And unlike selfish humans, he's willing to share.\"{EOL}—Garruk Wildspeaker")
        .Power(4)
        .Toughness(4)
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever another creature with power 3 or greater enters the battlefield under your control, you may draw a card.";

          p.Trigger(new OnZoneChanged(
            to: Zone.Battlefield,
            filter: (card, ability, _) => card.Is().Creature && ability.OwningCard.Controller == card.Controller));
          
          p.TriggerOnlyIfOwningCardIsInPlay = true;
          
          p.Effect = () => new DrawCards(1);
        });
    }
  }
}
