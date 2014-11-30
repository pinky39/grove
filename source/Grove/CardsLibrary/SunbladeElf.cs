namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.RepetitionRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class SunbladeElf : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sunblade Elf")
        .ManaCost("{G}")
        .Type("Creature — Elf Warrior")
        .Text(
          "Sunblade Elf gets +1/+1 as long as you control a Plains.{EOL}{4}{W}: Creatures you control get +1/+1 until end of turn.")
        .FlavorText("\"We patrol the steppes to keep enemies from the forest verge.\"")
        .Power(1)
        .Toughness(1)
        .StaticAbility(p =>
          {
            p.Modifier(() => new AddPowerAndToughness(1, 1));
            p.Condition = cond => cond.OwnerControlsPermanent(c => c.Is("plains"));
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{4}{W}: Creatures you control get +1/+1 until end of turn.";

            p.Cost = new PayMana("{4}{W}".Parse(), ManaUsage.Abilities, supportsRepetitions: true);

            p.Effect = () => new ApplyModifiersToPermanents((e, c) => c.Is().Creature, ControlledBy.SpellOwner,
              () => new AddPowerAndToughness(1, 1) {UntilEot = true});

            p.TimingRule(new Any(new AfterOpponentDeclaresAttackers(), new BeforeYouDeclareAttackers()));
            p.RepetitionRule(new RepeatMaxTimes());
          });
    }
  }
}