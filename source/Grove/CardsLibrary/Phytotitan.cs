namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Modifiers;
  using Triggers;

  public class Phytotitan : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Phytotitan")
        .ManaCost("{4}{G}{G}")
        .Type("Creature —  Plant Elemental")
        .Text(
          "When Phytotitan dies, return it to the battlefield tapped under its owner's control at the beginning of his or her next upkeep.")
        .FlavorText("Its root system spans the entire floor of the jungle, making eradication impossible.")
        .Power(7)
        .Toughness(2)
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Phytotitan dies, return it to the battlefield tapped under its owner's control at the beginning of his or her next upkeep.";

            p.Trigger(new OnZoneChanged(from: Zone.Battlefield, to: Zone.Graveyard));

            p.Effect = () => new ApplyModifiersToSelf(
              () =>
                {
                  var tp = new TriggeredAbility.Parameters
                    {
                      Effect = () => new PutOwnerToBattlefield(from: Zone.Graveyard, tap: true),
                    };

                  tp.Trigger(new OnStepStart(
                    step: Step.Upkeep,
                    onlyOnce: true,
                    passiveTurn: false,
                    activeTurn: true));

                  return new AddTriggeredAbility(new TriggeredAbility(tp));
                });
          });
    }
  }
}