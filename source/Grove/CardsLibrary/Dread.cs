namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Events;
  using Triggers;

  public class Dread : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Dread")
        .ManaCost("{3}{B}{B}{B}")
        .Type("Creature - Elemental Incarnation")
        .Text(
          "{Fear}{EOL}Whenever a creature deals damage to you, destroy it.{EOL}When Dread is put into a graveyard from anywhere, shuffle it into its owner's library.")
        .Power(6)
        .Toughness(6)
        .SimpleAbilities(Static.Fear)
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever a creature deals damage to you, destroy it.";

            p.Trigger(new OnDamageDealt(dmg =>
              dmg.IsDealtToYou &&                
                dmg.Source.Is().Creature));

            p.Effect = () => new DestroyPermanent(P(e =>
              e.TriggerMessage<DamageDealtEvent>().Damage.Source));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .TriggeredAbility(p =>
          {
            p.Text = "When Dread is put into a graveyard from anywhere, shuffle it into its owner's library.";
            p.Trigger(new OnZoneChanged(to: Zone.Graveyard));
            p.Effect = () => new ShuffleOwningCardIntoLibrary();
          });
    }
  }
}