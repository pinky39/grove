namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class ConeOfFlame : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Cone of Flame")
        .ManaCost("{3}{R}{R}")
        .Type("Sorcery")
        .Text(
          "Cone of Flame deals 1 damage to target creature or player, 2 damage to another target creature or player, and 3 damage to a third target creature or player.")
        .Cast(p =>
          {
            var amounts = new[] {1, 2, 3};
            p.Effect = () => new DealDifferentDamageToTargets(amounts);

            p.TargetSelector.AddEffect(trg =>
                {
                  trg.Is.CreatureOrPlayer().On.Battlefield();
                  trg.MinCount = 3;
                  trg.MaxCount = 3;
                });

            p.TargetingRule(new EffectDealDifferentDamage(amounts));                
          });
    }
  }
}