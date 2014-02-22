namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TargetingRules;
  using Grove.Gameplay.Effects;

  public class JaggedLightning : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Jagged Lightning")
        .ManaCost("{3}{R}{R}")
        .Type("Sorcery")
        .Text("Jagged Lightning deals 3 damage to each of two target creatures.")
        .FlavorText(
          "The pungent smell of roasting flesh made both mages realize they'd rather break for dinner than fight.")
        .Cast(p =>
          {
            p.Effect = () => new DealDamageToTargets(3);
            p.TargetSelector.AddEffect(trg =>
              {
                trg.Is.Creature().On.Battlefield();
                trg.MinCount = 2;
                trg.MaxCount = 2;
              });

            p.TargetingRule(new EffectDealDamage(3));
          });
    }
  }
}