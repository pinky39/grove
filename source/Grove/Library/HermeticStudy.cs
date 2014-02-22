namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Modifiers;

  public class HermeticStudy : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Hermetic Study")
        .ManaCost("{1}{U}")
        .Type("Enchantment Aura")
        .Text(
          "Enchanted creature has '{T}: This creature deals 1 damage to target creature or player.'")
        .FlavorText("Books can be replaced; a prize student cannot. Be patient.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(() =>
              {
                var ap = new ActivatedAbilityParameters
                  {
                    Text = "{T}: This creature deals 1 damage to target creature or player.",
                    Cost = new Tap(),
                    Effect = () => new DealDamageToTargets(1),
                  };

                ap.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());
                ap.TargetingRule(new EffectDealDamage(1));
                ap.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.DealDamage));

                return new AddActivatedAbility(new ActivatedAbility(ap));
              });

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new OnSecondMain());
            p.TargetingRule(new EffectRankBy(c => c.Score, ControlledBy.SpellOwner));
          });
    }
  }
}