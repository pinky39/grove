namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TargetingRules;
  using Core.Dsl;
  using Core.Effects;

  public class JaggedLightning : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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

            p.TargetingRule(new DealDamage(3));
          });
    }
  }
}