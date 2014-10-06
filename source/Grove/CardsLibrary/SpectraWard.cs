namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class SpectraWard : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Spectra Ward")
        .ManaCost("{3}{W}{W}")
        .Type("Enchantment - Aura")
        .Text("Enchant creature{EOL}Enchanted creature gets +2/+2 and has protection from all colors. This effect doesn't remove Auras. {I}(It can't be blocked, targeted, or dealt damage by anything that's white, blue, black, red, or green.){/I}")
        .Cast(p =>
        {
          p.Effect = () => new Attach(
            () => new AddPowerAndToughness(2, 2),
            () => new AddProtectionFromColors(L(CardColor.Black, CardColor.Blue, CardColor.Green, CardColor.Red, CardColor.White)))
            .SetTags(EffectTag.IncreasePower, EffectTag.IncreasePower);

          p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
          p.TimingRule(new OnFirstMain());
          p.TargetingRule(new EffectCombatEnchantment());
        });
    }
  }
}
