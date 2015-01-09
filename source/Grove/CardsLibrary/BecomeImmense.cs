namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class BecomeImmense : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Become Immense")
        .ManaCost("{5}{G}")
        .Type("Instant")
        .Text("{Delve}{I}(Each card you exile from your graveyard while casting this spell pays for {1}.){/I}{EOL}Target creature gets +6/+6 until end of turn.")
        .SimpleAbilities(Static.Delve)
        .Cast(p =>
        {
          p.Effect = () => new ApplyModifiersToTargets(
              () => new AddPowerAndToughness(6, 6) { UntilEot = true }).SetTags(EffectTag.IncreasePower,
                EffectTag.IncreaseToughness);

          p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
          p.TimingRule(new PumpTargetCardTimingRule());
          p.TargetingRule(new EffectPumpInstant(6, 6));
        });
    }
  }
}
