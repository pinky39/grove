namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.Events;
  using Grove.AI.TimingRules;
  using Grove.Triggers;

  public class AngelicChorus : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Angelic Chorus")
        .ManaCost("{3}{W}{W}")
        .Type("Enchantment")
        .Text("Whenever a creature enters the battlefield under your control, you gain life equal to its toughness.")
        .FlavorText("The very young and the very old know best the song the angels sing.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever a creature enters the battlefield under your control, you gain life equal to its toughness.";
            p.Trigger(new OnZoneChanged(
              to: Zone.Battlefield,
              filter: (c, a, g) => a.OwningCard.Controller == c.Controller && c.Is().Creature));
            p.Effect = () => new ChangeLife(
              amount: P(e => e.TriggerMessage<ZoneChangedEvent>().Card.Toughness.GetValueOrDefault()), yours: true);
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          }
        );
    }
  }
}