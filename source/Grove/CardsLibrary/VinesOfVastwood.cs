namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI;
  using Grove.AI.TargetingRules;
  using Grove.Modifiers;

  public class VinesOfVastwood : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Vines of Vastwood")
        .ManaCost("{G}")
        .Type("Instant")
        .Text(
          "{Kicker} {G}{EOL}Target creature can't be the target of spells or abilities your opponents control this turn. If Vines of Vastwood was kicked, that creature gets +4/+4 until end of turn.")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToTargets(() => new AddStaticAbility(Static.Hexproof) {UntilEot = true})
              .SetTags(EffectTag.Shroud);

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new EffectProtect());
          })
        .Cast(p =>
          {
            p.Text = p.KickerDescription;
            p.Cost = new PayMana("{G}{G}".Parse());
            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddStaticAbility(Static.Hexproof) {UntilEot = true},
              () => new AddPowerAndToughness(4, 4) {UntilEot = true})
              .SetTags(EffectTag.Shroud, EffectTag.IncreasePower, EffectTag.IncreaseToughness);

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            
            p.TimingRule(new PumpTargetCardTimingRule());
            p.TargetingRule(new EffectPumpInstant(4, 4));
          });
    }
  }
}