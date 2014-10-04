namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using Effects;
  using Modifiers;
  using Triggers;

  public class AeronautTinkerer : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Aeronaut Tinkerer")
        .ManaCost("{2}{U}")
        .Type("Creature — Human Artificer")
        .Text(
          "Aeronaut Tinkerer has flying as long as you control an artifact.{I}(It can't be blocked except by creatures with flying or reach.){/I}")
        .FlavorText("\"All tinkerers have their heads in the clouds. I don't intend to stop there.\"")
        .Power(2)
        .Toughness(3)
        .TriggeredAbility(p =>
        {
          p.Trigger(new OnZoneChanged(
            to: Zone.Battlefield,
            filter: (card, ability, _) =>
            {
              var count = ability.OwningCard.Controller.Battlefield.Count(c => c.Is().Artifact);

              // Aeronaut Tinkerer comes into battlefield
              if (ability.OwningCard == card && count > 0)
                return true;

              return ability.OwningCard.Zone == Zone.Battlefield &&
                     ability.OwningCard.Controller == card.Controller && card.Is().Artifact && count == 1;
            }));

          p.UsesStack = false;

          p.Effect = () => new ApplyModifiersToSelf(
            () =>
            {
              var modifier = new AddStaticAbility(Static.Flying);
              modifier.AddLifetime(new OwnerControlsPermamentsLifetime(c => c.Is().Artifact));
              return modifier;
            });
        });
    }
  }
}
