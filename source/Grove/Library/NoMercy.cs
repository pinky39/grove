namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Messages;
  using Grove.Gameplay.Triggers;

  public class NoMercy : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("No Mercy")
        .ManaCost("{2}{B}{B}")
        .Type("Enchantment")
        .Text("Whenever a creature deals damage to you, destroy it.")
        .FlavorText("We had years to prepare, while they had mere minutes.")
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever a creature deals damage to you, destroy it.";

            p.Trigger(new OnDamageDealt(
              onlyByTriggerSource: false,
              playerFilter: (ply, trg, dmg) =>
                ply == trg.OwningCard.Controller && dmg.Source.Is().Creature));

            p.Effect = () => new DestroyPermanent(P(e =>
              e.TriggerMessage<DamageHasBeenDealt>().Damage.Source));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}