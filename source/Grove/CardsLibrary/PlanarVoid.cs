namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.Events;
  using Grove.AI.TimingRules;
  using Grove.Triggers;

  public class PlanarVoid : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Planar Void")
        .ManaCost("{B}")
        .Type("Enchantment")
        .Text("Whenever another card is put into a graveyard from anywhere, exile that card.")
        .FlavorText("'Planeswalking isn't about walking. It's about falling and screaming.'")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever another card is put into a graveyard from anywhere, exile that card.";

            p.Trigger(new OnZoneChanged(
              to: Zone.Graveyard,
              filter: delegate { return true; }));

            p.Effect = () => new ExileCard(P(e => e.TriggerMessage<ZoneChanged>().Card), Zone.Graveyard);
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}