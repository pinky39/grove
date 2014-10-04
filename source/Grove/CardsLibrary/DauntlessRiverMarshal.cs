namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;
  using Triggers;

  public class DauntlessRiverMarshal : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
          .Named("Dauntless River Marshal")
          .ManaCost("{1}{W}")
          .Type("Creature — Human Soldier")
          .Text("Dauntless River Marshal gets +1/+1 as long as you control an Island.{EOL}{3}{U}: Tap target creature.")
          .FlavorText("\"Thieves and squid squirm the same way when you capture them.\"")
          .Power(2)
          .Toughness(1)
          .TriggeredAbility(p =>
          {
            p.Trigger(new OnZoneChanged(
              to: Zone.Battlefield,
              filter: (card, ability, _) =>
              {
                var count = ability.OwningCard.Controller.Battlefield.Count(c => c.Is().OfType("Island"));

                // Dauntless River Marshal comes into battlefield
                if (ability.OwningCard == card && count > 0)
                  return true;

                return ability.OwningCard.Zone == Zone.Battlefield &&
                  ability.OwningCard.Controller == card.Controller && card.Is().OfType("Island") && count == 1;
              }));

            p.UsesStack = false;

            p.Effect = () => new ApplyModifiersToSelf(
              () =>
              {
                var modifier = new AddPowerAndToughness(1, 1);
                modifier.AddLifetime(new OwnerControlsPermamentsLifetime(c => c.Is().OfType("Island")));
                return modifier;
              });
          })
          .ActivatedAbility(p =>
          {
            p.Text = "{3}{U}: Tap target creature.";

            p.Cost = new PayMana("{3}{U}".Parse(), ManaUsage.Abilities);

            p.Effect = () => new TapTargets();

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TargetingRule(new EffectTapCreature());
            p.TimingRule(new Any(new AfterYouDeclareAttackers(), new OnOpponentsTurn(Step.FirstMain)));
          });
    }
  }
}
