namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Abilities;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class Reflexes : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Reflexes")
        .ManaCost("{R}")
        .Type("Enchantment Aura")
        .Text("Enchanted creature has first strike.")
        .FlavorText("Here's how ya win. Don't let the other guy hit back first.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(
              () => new AddStaticAbility(Static.FirstStrike));

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new FirstMain());
            p.TargetingRule(new CombatEnchantment());
          });
    }
  }
}