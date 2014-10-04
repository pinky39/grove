namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using Effects;
  using Modifiers;
  using Triggers;

  public class ScrapyardMongrel : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
          .Named("Scrapyard Mongrel")
          .ManaCost("{3}{R}")
          .Type("Creature — Hound")
          .Text("As long as you control an artifact, Scrapyard Mongrel gets +2/+0 and has trample.{I}(If it would assign enough damage to its blockers to destroy them, you may have it assign the rest of its damage to defending player or planeswalker.){/I}")
          .FlavorText("Trespassers are welcome to try.")
          .Power(3)
          .Toughness(3)
          .TriggeredAbility(p =>
          {
            p.Trigger(new OnZoneChanged(
              to: Zone.Battlefield,
              filter: (card, ability, _) =>
              {
                var count = ability.OwningCard.Controller.Battlefield.Count(c => c.Is().Artifact);

                // Scrapyard Mongrel comes into battlefield
                if (ability.OwningCard == card && count > 0)
                  return true;

                return ability.OwningCard.Zone == Zone.Battlefield &&
                       ability.OwningCard.Controller == card.Controller && card.Is().Artifact && count == 1;
              }));

            p.UsesStack = false;

            p.Effect = () => new ApplyModifiersToSelf(
              () =>
              {
                var modifier = new AddPowerAndToughness(2, 0);
                modifier.AddLifetime(new OwnerControlsPermamentsLifetime(c => c.Is().Artifact));
                return modifier;
              },
              () =>
              {
                var modifier = new AddStaticAbility(Static.Trample);
                modifier.AddLifetime(new OwnerControlsPermamentsLifetime(c => c.Is().Artifact));
                return modifier;
              });
          });
    }
  }
}
