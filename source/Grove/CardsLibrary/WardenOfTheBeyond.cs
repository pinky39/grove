namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using Effects;
  using Modifiers;
  using Triggers;

  public class WardenOfTheBeyond : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Warden of the Beyond")
        .ManaCost("{2}{W}")
        .Type("Creature — Human Wizard")
        .Text("{Vigilance} {I}(Attacking doesn't cause this creature to tap.){/I}{EOL}Warden of the Beyond gets +2/+2 as long as an opponent owns a card in exile.")
        .FlavorText("He draws strength from a vast source few mortals can fathom.")
        .Power(2)
        .Toughness(2)
        .SimpleAbilities(Static.Vigilance)
        .TriggeredAbility(p =>
        {
          p.Trigger(new OnZoneChanged(
            to: Zone.Battlefield,
            filter: (card, ability, _) =>
            {
              var count = ability.OwningCard.Controller.Exile.Count();

              // Warden of the Beyond comes into battlefield
              return (ability.OwningCard == card && count > 0);
            }));

          p.UsesStack = false;

          p.Effect = () => new ApplyModifiersToSelf(
            () =>
            {
              var modifier = new AddPowerAndToughness(2, 2);
              modifier.AddLifetime(new OwnerHasCardsInExile(selector: null));
              return modifier;
            }).SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);
        })
        .TriggeredAbility(p =>
        {
          p.Trigger(new OnZoneChanged(
            to: Zone.Exile,
            filter: (card, ability, _) =>
            {
              var count = ability.OwningCard.Controller.Exile.Count();

              if (ability.OwningCard == card)
                return false;

              return ability.OwningCard.Zone == Zone.Battlefield &&
                     ability.OwningCard.Controller == card.Controller && count == 1;
            }));

          p.UsesStack = false;

          p.Effect = () => new ApplyModifiersToSelf(
            () =>
            {
              var modifier = new AddPowerAndToughness(2, 2);
              modifier.AddLifetime(new OwnerHasCardsInExile(selector: null));
              return modifier;
            }).SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);
        });
    }
  }
}
