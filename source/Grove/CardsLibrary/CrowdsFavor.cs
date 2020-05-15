namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class CrowdsFavor : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Crowd's Favor")
        .ManaCost("{R}")
        .Type("Instant")
        .Text(
          "{Convoke} {I}(Your creatures can help cast this spell. Each creature you tap while casting this spell pays for {1} or one mana of that creature's color.){/I}{EOL}Target creature gets +1/+0 and gains first strike until end of turn. {I}(It deals combat damage before creatures without first strike.){/I}")
        .SimpleAbilities(Static.Convoke)
        .Cast(p =>
          {
            p.Text = "Target creature gets +1/+0 and gains first strike until end of turn.";

            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddPowerAndToughness(1, 0) {UntilEot = true},
              () => new AddSimpleAbility(Static.FirstStrike) {UntilEot = true}).SetTags(
                EffectTag.IncreasePower);

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TimingRule(new Any(
              new BeforeYouDeclareAttackers(),
              new AfterOpponentDeclaresAttackers()));
            p.TargetingRule(new EffectPumpInstant(1, 0));
          });
    }
  }
}