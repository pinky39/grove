namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Characteristics;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class MaskOfLawAndGrace : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Mask of Law and Grace")
        .ManaCost("{W}")
        .Type("Enchantment Aura")
        .Text("Enchanted creature has protection from black and from red.")
        .FlavorText(
          "The archangels zealously drove Serra's light into every corner of their new home as if their creator still commanded them.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(
              () => new AddProtectionFromColors(L(CardColor.Black, CardColor.Red)));

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCombatEnchantment());
          });
    }
  }
}