namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class MidnightGuard : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
          .Named("Midnight Guard")
          .ManaCost("{2}{W}")
          .Type("Creature — Human Soldier")
          .Text("Whenever another creature enters the battlefield, untap Midnight Guard.")
          .FlavorText("\"When you're on watch, no noise is harmless and no shadow can be ignored.\"{EOL}—Olgard of the Skiltfolk")
          .Power(2)
          .Toughness(3)
          .TriggeredAbility(p =>
          {
            p.Text = "Whenever another creature enters the battlefield, untap Midnight Guard.";

            p.Trigger(new OnZoneChanged(
                to: Zone.Battlefield,
                filter: (card, ability, game) => card.Is().Creature && card != ability.OwningCard));

            p.Effect = () => new UntapOwner();

            p.TriggerOnlyIfOwningCardIsInPlay = true;
            p.UsesStack = false;
          });
    }
  }
}
