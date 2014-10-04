namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using Effects;
  using Modifiers;
  using Triggers;

  public class GlacialCrasher : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Glacial Crasher")
        .ManaCost("{4}{U}{U}")
        .Type("Creature — Elemental")
        .Text(
          "{Trample}{I}(If this creature would assign enough damage to its blockers to destroy them, you may have it assign the rest of its damage to defending player or planeswalker.){/I}{EOL}Glacial Crasher can't attack unless there is a Mountain on the battlefield.")
        .Power(5)
        .Toughness(5)
        .SimpleAbilities(Static.CannotAttack)
        .TriggeredAbility(p =>
        {
          p.Trigger(new OnZoneChanged(
            to: Zone.Battlefield,
            filter: (card, ability, _) =>
            {
              var count = ability.OwningCard.Controller.Battlefield.Count(c => c.Is().OfType("Mountain"));

              // Glacial Crasher comes into battlefield
              if (ability.OwningCard == card && count > 0)
                return true;

              return ability.OwningCard.Zone == Zone.Battlefield &&
                     ability.OwningCard.Controller == card.Controller && card.Is().OfType("Mountain") && count == 1;
            }));

          p.UsesStack = false;

          p.Effect = () => new ApplyModifiersToSelf(
            () =>
            {
              var modifier = new DisableAbility(Static.CannotAttack);
              modifier.AddLifetime(new OwnerControlsPermamentsLifetime(c => c.Is().OfType("Mountain")));
              return modifier;
            });
        });
    }
  }
}
