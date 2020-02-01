namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class AwakenTheBear : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Awaken the Bear")
        .ManaCost("{2}{G}")
        .Type("Instant")
        .Text("Target creature gets +3/+3 and gains trample until end of turn.")
        .FlavorText("When Temur warriors enter the battle trance known as \"awakening the bear,\" they lose all sense of enemy or friend, seeing only threats to the wilderness.")
        .Cast(p =>
        {
          p.Effect = () => new ApplyModifiersToTargets(
            () => new AddPowerAndToughness(3, 3) { UntilEot = true },
            () => new AddStaticAbility(Static.Trample) { UntilEot = true })
              .SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);

          p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
          p.TimingRule(new PumpTargetCardTimingRule());
          p.TargetingRule(new EffectPumpInstant(3, 3));
        });
    }
  }
}
