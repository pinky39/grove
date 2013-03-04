namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;
  using Core.Effects;
  using Core.Messages;
  using Core.Triggers;
  using Core.Zones;

  public class Dread : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Dread")
        .ManaCost("{3}{B}{B}{B}")
        .Type("Creature - Elemental Incarnation")
        .Text(
          "{Fear}{EOL}Whenever a creature deals damage to you, destroy it.{EOL}When Dread is put into a graveyard from anywhere, shuffle it into its owner's library.")
        .Power(6)
        .Toughness(6)
        .StaticAbilities(Static.Fear)
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever a creature deals damage to you, destroy it.";

            p.Trigger(new OnDamageDealt(
              onlyByTriggerSource: false,
              playerFilter: (ply, trg, dmg) =>
                ply == trg.OwningCard.Controller && dmg.Source.Is().Creature));

            p.Effect = () => new DestroyPermanent(P(e =>
              e.TriggerMessage<DamageHasBeenDealt>().Damage.Source));
          })
        .TriggeredAbility(p =>
          {
            p.Text = "When Dread is put into a graveyard from anywhere, shuffle it into its owner's library.";
            p.Trigger(new OnZoneChanged(to: Zone.Graveyard));
            p.Effect = () => new ShuffleIntoLibrary();
          });
    }
  }
}