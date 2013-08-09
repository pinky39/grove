namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Characteristics;
  using Gameplay.Effects;
  using Gameplay.Messages;
  using Gameplay.Misc;
  using Gameplay.Triggers;
  using Gameplay.Zones;

  public class Bereavement : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Bereavement")
        .ManaCost("{1}{B}")
        .Type("Enchantment")
        .Text("Whenever a green creature dies, its controller discards a card.")
        .FlavorText("Grief is as useless as love.")
        .Cast(p => p.TimingRule(new FirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever a green creature dies, its controller discards a card.";

            p.Trigger(new OnZoneChanged(
              from: Zone.Battlefield, to: Zone.Graveyard,
              filter: (c, a , g) => c.Is().Creature && c.HasColor(CardColor.Green)));

            p.Effect = () => new DiscardCards(
              count: 1,
              player: P(e => e.TriggerMessage<ZoneChanged>().Card.Controller));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          }
        );
    }
  }
}