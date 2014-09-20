namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;

  public class MantisEngine : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Mantis Engine")
        .ManaCost("{5}")
        .Type("Artifact Creature Insect")
        .Text(
          "{2}: Mantis Engine gains flying until end of turn.{EOL}{2}: Mantis Engine gains first strike until end of turn.")
        .FlavorText("Tawnos left a legacy of animal designs in many of Urza's creations")
        .Power(3)
        .Toughness(3)
        .ActivatedAbility(p =>
          {
            p.Text = "{2}: Mantis Engine gains flying until end of turn.";
            p.Cost = new PayMana(2.Colorless(), ManaUsage.Abilities);
            p.Effect = () => new ApplyModifiersToSelf(
              () => new AddStaticAbility(Static.Flying) {UntilEot = true});

            p.TimingRule(new Any(
              new BeforeYouDeclareAttackers(),
              new AfterOpponentDeclaresAttackers()));

            p.TimingRule(new WhenCardHas(c => !c.Has().Flying));
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{2}: Mantis Engine gains first strike until end of turn.";
            p.Cost = new PayMana(2.Colorless(), ManaUsage.Abilities);
            p.Effect = () => new ApplyModifiersToSelf(
              () => new AddStaticAbility(Static.FirstStrike) {UntilEot = true});

            p.TimingRule(new Any(
              new AfterOpponentDeclaresAttackers(),
              new AfterOpponentDeclaresBlockers()));

            p.TimingRule(new WhenCardHas(c => !c.Has().FirstStrike));
          });
    }
  }
}