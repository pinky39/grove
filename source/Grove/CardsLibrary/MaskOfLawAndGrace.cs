namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;

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