namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Abilities;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class HermeticStudy : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
                ap.TargetingRule(new DealDamage(1));
                ap.TimingRule(new TargetRemoval());

                return new AddActivatedAbility(new ActivatedAbility(ap));
              });

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new SecondMain());
            p.TargetingRule(new OrderByRank(c => c.Score));
          });
    }
  }
}