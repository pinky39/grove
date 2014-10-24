namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class HoardingDragon : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Hoarding Dragon")
        .ManaCost("{3}{R}{R}")
        .Type("Creature — Dragon")
        .Text(
          "{Flying}{EOL}When Hoarding Dragon enters the battlefield, you may search your library for an artifact card, exile it, then shuffle your library.{EOL}When Hoarding Dragon dies, you may put the exiled card into its owner's hand.")
        .Power(4)
        .Toughness(4)
        .SimpleAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Hoarding Dragon enters the battlefield, you may search your library for an artifact card, exile it, then shuffle your library.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

            p.Effect = () =>
              new SearchLibraryPutToZone(
                zone: Zone.Exile,
                minCount: 0,
                maxCount: 1,
                validator: (e, c) => c.Is().Artifact,
                text: "Search your library for an artifact.",
                afterPutToZone: (card, effect) => card.Attach(effect.Source.OwningCard));
          })
        .TriggeredAbility(p =>
          {
            p.Text = "When Hoarding Dragon dies, you may put the exiled card into its owner's hand.";

            p.Trigger(new OnZoneChanged(
              from: Zone.Battlefield,
              to: Zone.Graveyard,
              filter: (c, a, g) => a.OwningCard == c && a.OwningCard.AttachedTo != null));

            p.Effect = () => new ReturnToHand(P(e => e.Source.OwningCard.AttachedTo));
          });
    }
  }
}