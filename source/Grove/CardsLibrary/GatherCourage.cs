namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class GatherCourage : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Gather Courage")
        .ManaCost("{G}")
        .Type("Instant")
        .Text(
          "{Convoke}{I}(Your creatures can help cast this spell. Each creature you tap while casting this spell pays for {1} or one mana of that creature's color.){/I}{EOL}Target creature gets +2/+2 until end of turn.")
        .FlavorText("\"Even your shadow is too foul to tolerate.\"")
        .SimpleAbilities(Static.Convoke)
        .Cast(p =>
          {
            p.Text = "Target creature gets +2/+2 until end of turn.";

            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddPowerAndToughness(2, 2) {UntilEot = true});

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TargetingRule(new EffectPumpInstant(2, 2));
            p.TimingRule(new PumpTargetCardTimingRule());
          });
    }
  }
}