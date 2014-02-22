namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;

  public class ThornwindFaeries : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Thornwind Faeries")
        .ManaCost("{1}{U}{U}")
        .Type("Creature Faerie")
        .Text("{Flying}{EOL}{T}: Thornwind Faeries deals 1 damage to target creature or player.")
        .FlavorText(
          "Guarding the ship is the Thornwinds' first concern. Getting along with the locals ranks fourth or fifth at best.")
        .Power(1)
        .Toughness(1)
        .SimpleAbilities(Static.Flying)
        .ActivatedAbility(p =>
          {
            p.Text = "{T}: Thornwind Faeries deals 1 damage to target creature or player.";
            p.Cost = new Tap();
            p.Effect = () => new DealDamageToTargets(1);

            p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());
            p.TargetingRule(new EffectDealDamage(1));
            p.TimingRule(new TargetRemovalTimingRule(EffectTag.DealDamage));
          });
    }
  }
}