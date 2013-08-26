namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Artifical;
  using Artifical.RepetitionRules;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Abilities;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class ShivsEmbrace : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Shiv's Embrace")
        .ManaCost("{2}{R}{R}")
        .Type("Enchantment - Aura")
        .Text(
          "Enchanted creature gets +2/+2 and has flying.{EOL}{R}: Enchanted creature gets +1/+0 until end of turn.")
        .FlavorText("Wear the foe's form to best it in battle. So sayeth the bey.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(
              () =>
                {
                  var ap = new ActivatedAbilityParameters
                    {
                      Text = "{R}: Enchanted creature gets +1/+0 until end of turn.",
                      Cost = new PayMana(Mana.Red, ManaUsage.Abilities, supportsRepetitions: true),
                      Effect = () => new ApplyModifiersToSelf(() => new AddPowerAndToughness(1, 0) {UntilEot = true})
                    };

                  ap.TimingRule(new PumpOwningCardTimingRule(1, 0));
                  ap.RepetitionRule(new RepeatMaxTimes());
                  return new AddActivatedAbility(new ActivatedAbility(ap));
                },
              () => new AddPowerAndToughness(2, 2),
              () => new AddStaticAbility(Static.Flying)) {Category = EffectCategories.ToughnessIncrease};

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCombatEnchantment());
          });
    }
  }
}