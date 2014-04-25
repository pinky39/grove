namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class SerraAdvocate : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Serra Advocate")
        .ManaCost("{3}{W}")
        .Type("Creature Angel")
        .Text("{Flying}{EOL}{T}: Target attacking or blocking creature gets +2/+2 until end of turn.")
        .FlavorText("An angel's touch can prepare a soldier for battle more than a thousand military drills.")
        .Power(2)
        .Toughness(2)
        .SimpleAbilities(Static.Flying)
        .ActivatedAbility(p =>
          {
            p.Text = "{T}: Target attacking or blocking creature gets +2/+2 until end of turn.";
            p.Cost = new Tap();
            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddPowerAndToughness(2, 2) {UntilEot = true}).SetTags(EffectTag.IncreasePower,
                EffectTag.IncreaseToughness);
            p.TargetSelector.AddEffect(trg => trg.Is.AttackerOrBlocker().On.Battlefield());
            p.TimingRule(new OnStep(Step.DeclareBlockers));
            p.TargetingRule(new EffectPumpAttackerOrBlocker(2, 2));
          });
    }
  }
}