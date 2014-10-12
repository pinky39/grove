namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using AI.TargetingRules;
  using Costs;
  using Effects;
  using Modifiers;
  using Triggers;

  public class JorubaiMurkLurker : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
          .Named("Jorubai Murk Lurker")
          .ManaCost("{2}{U}")
          .Type("Creature — Leech")
          .Text("Jorubai Murk Lurker gets +1/+1 as long as you control a Swamp.{EOL}{1}{B}: Target creature gains lifelink until end of turn. {I}(Damage dealt by the creature also causes its controller to gain that much life.){/I}")
          .Power(1)
          .Toughness(3)
          .TriggeredAbility(p =>
          {
            p.Trigger(new OnZoneChanged(
              to: Zone.Battlefield,
              filter: (card, ability, _) =>
              {
                var count = ability.OwningCard.Controller.Battlefield.Count(c => c.Is("Swamp"));

                // Jorubai Murk Lurker comes into battlefield
                if (ability.OwningCard == card && count > 0)
                  return true;

                return ability.OwningCard.Zone == Zone.Battlefield &&
                  ability.OwningCard.Controller == card.Controller && card.Is("Swamp") && count == 1;
              }));

            p.UsesStack = false;

            p.Effect = () => new ApplyModifiersToSelf(
              () =>
              {
                var modifier = new AddPowerAndToughness(1, 1);
                modifier.AddLifetime(new OwnerControlsPermamentsLifetime(c => c.Is("Swamp")));
                return modifier;
              });
          })
          .ActivatedAbility(p =>
          {
            p.Text = "{1}{B}: Target creature gains lifelink until end of turn.";

            p.Cost = new PayMana("{1}{B}".Parse(), ManaUsage.Abilities);

            p.Effect = () => new ApplyModifiersToTargets(() => new AddStaticAbility(Static.Lifelink) { UntilEot = true });

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TargetingRule(new EffectOrCostRankBy(c => c.Score));
          });
    }
  }
}
