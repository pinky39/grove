namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class Quickling : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Quickling")
        .ManaCost("{1}{U}")
        .Type("Creature — Faerie Rogue")
        .Text(
          "{Flash} {I}(You may cast this spell any time you could cast an instant.){/I}{EOL}{Flying}{EOL}When Quickling enters the battlefield, sacrifice it unless you return another creature you control to its owner's hand.")
        .Power(2)
        .Toughness(2)
        .SimpleAbilities(Static.Flash, Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Quickling enters the battlefield, sacrifice it unless you return another creature you control to its owner's hand.";

            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

            p.Effect = () => new ApplyActionToPermanentOrApplyActionToOwner(
              validator: c => c.Is().Creature,
              actionToPermament: (controller, card) => controller.PutCardToHand(card),
              actionToOwner: (controller, card) => card.Sacrifice(),
              canSelectSelf: false,
              shouldPayAi: (controller, card) => true,
              text: "Select a creature to return it to hand.",
              instructions: "(Press Enter to sacrifice Quickling.)");
          });
    }
  }
}