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
    private static Lifetime UntilControls()
    {
      return new OwnerControlsPermamentsLifetime(c => c.Is().OfType("Island"));
    }

    private static Effect GetEffect()
    {
      return new ApplyModifiersToSelf(
          () =>
          {
            var modifier = new AddPowerAndToughness(1, 1);
            modifier.AddLifetime(UntilControls());
            return modifier;
          });
    }

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
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield)
            {
              Condition = (t, g) => t.OwningCard.Controller.Battlefield.Count(c => c.Is().OfType("Island")) > 0
            });

            p.Effect = GetEffect;
            p.UsesStack = false;
          })
          .TriggeredAbility(p =>
          {
            p.Trigger(new OnZoneChanged(
                to: Zone.Battlefield,
                filter: (card, ability, game) => card.Is().OfType("Island"))
            {
              Condition = (t, g) => t.OwningCard.Controller.Battlefield.Count(c => c.Is().OfType("Island")) == 1
            });

            p.Effect = GetEffect;

            p.UsesStack = false;
            p.TriggerOnlyIfOwningCardIsInPlay = true;
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
