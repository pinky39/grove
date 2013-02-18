namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Ai.TargetingRules;
  using Core.Ai.TimingRules;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Modifiers;

  public class ShivsEmbrace : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Shiv's Embrace")
        .ManaCost("{2}{R}{R}")
        .Type("Enchantment - Aura")
        .Text(
          "{Enchant creature}{EOL}Enchanted creature gets +2/+2 and has flying.{EOL}{R}: Enchanted creature gets +1/+0 until end of turn.")
        .FlavorText("Wear the foe's form to best it in battle. So sayeth the bey.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(
              () =>
                {
                  var ap = new ActivatedAbilityParameters
                    {
                      Text = "{R}: Enchanted creature gets +1/+0 until end of turn.",
                      Cost = new PayMana(ManaAmount.Red, ManaUsage.Abilities),
                      Effect = () => new ApplyModifiersToSelf(() => new AddPowerAndToughness(1, 0) {UntilEot = true})
                    };

                  ap.TimingRule(new IncreaseOwnersPowerOrToughness(1, 0));
                  return new AddActivatedAbility(new ActivatedAbility(ap));
                },
              () => new AddPowerAndToughness(2, 2),
              () => new AddStaticAbility(Static.Flying)) {Category = EffectCategories.ToughnessIncrease};

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new FirstMain());
            p.TargetingRule(new CombatEnchantment());
          });
    }
  }
}