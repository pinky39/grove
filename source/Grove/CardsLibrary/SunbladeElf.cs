namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using AI.RepetitionRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;
  using Triggers;

  public class SunbladeElf : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
          .Named("Sunblade Elf")
          .ManaCost("{G}")
          .Type("Creature — Elf Warrior")
          .Text("Sunblade Elf gets +1/+1 as long as you control a Plains.{EOL}{4}{W}: Creatures you control get +1/+1 until end of turn.")
          .FlavorText("\"We patrol the steppes to keep enemies from the forest verge.\"")
          .Power(1)
          .Toughness(1)
          .TriggeredAbility(p =>
          {
            p.Trigger(new OnZoneChanged(
              to: Zone.Battlefield,
              filter: (card, ability, _) =>
              {
                var count = ability.OwningCard.Controller.Battlefield.Count(c => c.Is("Plains"));

                // Sunblade Elf comes into battlefield
                if (ability.OwningCard == card && count > 0)
                  return true;

                return ability.OwningCard.Zone == Zone.Battlefield &&
                  ability.OwningCard.Controller == card.Controller && card.Is("Plains") && count == 1;
              }));

            p.UsesStack = false;

            p.Effect = () => new ApplyModifiersToSelf(
              () =>
              {
                var modifier = new AddPowerAndToughness(1, 1);
                modifier.AddLifetime(new OwnerControlsPermamentsLifetime(c => c.Is("Plains")));
                return modifier;
              });
          })
          .ActivatedAbility(p =>
          {
            p.Text = "{4}{W}: Creatures you control get +1/+1 until end of turn.";

            p.Cost = new PayMana("{4}{W}".Parse(), ManaUsage.Abilities, supportsRepetitions: true);

            p.Effect = () => new ApplyModifiersToPermanents((e, c) => c.Is().Creature, ControlledBy.SpellOwner,
                () => new AddPowerAndToughness(1, 1) { UntilEot = true });

            p.TimingRule(new WhenYouHavePermanents(c => c.Is().Creature, 2));
            p.TimingRule(new Any(new AfterOpponentDeclaresAttackers(), new BeforeYouDeclareAttackers(), new OnEndOfOpponentsTurn()));
            p.RepetitionRule(new RepeatMaxTimes());
          });
    }
  }
}
