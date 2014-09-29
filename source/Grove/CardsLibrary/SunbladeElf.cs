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
    private static Lifetime UntilControls()
    {
      return new OwnerControlsPermamentsLifetime(c => c.Is().OfType("Plains"));
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
          .Named("Sunblade Elf")
          .ManaCost("{G}")
          .Type("Creature — Elf Warrior")
          .Text("Sunblade Elf gets +1/+1 as long as you control a Plains.{EOL}{4}{W}: Creatures you control get +1/+1 until end of turn.")
          .FlavorText("\"We patrol the steppes to keep enemies from the forest verge.\"")
          .Power(1)
          .Toughness(1)
          .TriggeredAbility(p =>
          {
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield)
            {
              Condition = (t, g) => t.OwningCard.Controller.Battlefield.Count(c => c.Is().OfType("Plains")) > 0
            });

            p.Effect = GetEffect;
            p.UsesStack = false;
          })
          .TriggeredAbility(p =>
          {
            p.Trigger(new OnZoneChanged(
                to: Zone.Battlefield,
                filter: (card, ability, game) => card.Is().OfType("Plains"))
            {
              Condition = (t, g) => t.OwningCard.Controller.Battlefield.Count(c => c.Is().OfType("Plains")) == 1
            });

            p.Effect = GetEffect;

            p.UsesStack = false;
            p.TriggerOnlyIfOwningCardIsInPlay = true;
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
