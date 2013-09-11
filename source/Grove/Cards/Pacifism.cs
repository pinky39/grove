namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Artifical;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Abilities;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class Pacifism : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Pacifism")
        .ManaCost("{1}{W}")
        .Type("Enchantment - Aura")
        .Text("Enchanted creature can't attack or block.")
        .FlavorText("Fight? I cannot. I do not care if I live or die, so long as I can rest.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(
              () => new AddStaticAbility(Static.CannotBlock),
              () => new AddStaticAbility(Static.CannotAttack)).Tags(EffectTag.CombatDisabler);              

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCannotBlockAttack());
          });
    }
  }
}