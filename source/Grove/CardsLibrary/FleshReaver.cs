namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.Events;
  using Grove.Triggers;

  public class FleshReaver : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Flesh Reaver")
        .ManaCost("{1}{B}")
        .Type("Creature Horror")
        .Text(
          "Whenever Flesh Reaver deals damage to a creature or opponent, Flesh Reaver deals that much damage to you.")
        .FlavorText(
          "Though the reaver is horrifyingly effective, its dorsal vents spit a highly corrosive cloud of filth.")
        .Power(4)
        .Toughness(4)
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever Flesh Reaver deals damage to a creature or opponent, Flesh Reaver deals that much damage to you.";
            p.Trigger(new OnDamageDealt(
              creatureFilter: delegate { return true; },
              playerFilter: (player, tr, _) => tr.Ability.OwningCard.Controller != player));

            p.Effect = () => new DealExistingDamageToController(P(e => e.TriggerMessage<DamageHasBeenDealt>().Damage));
          });
    }
  }
}