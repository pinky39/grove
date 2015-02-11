namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class OppressiveRays : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Oppressive Rays")
        .ManaCost("{W}")
        .Type("Enchantment - Aura")
        .Text(
          "Enchant creature{EOL}Enchanted creature can't attack or block unless its controller pays {3}.{EOL}Activated abilities of enchanted creature cost {3} more to activate.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(
              () => new IncreaseCombatCost(3),
              () => new AddCostModifier(new ChangeManaCostOfEnchantedCreaturesAbilities(3)))
              .SetTags(EffectTag.CombatDisabler);

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCannotBlockAttack());
          });
    }
  }
}