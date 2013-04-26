namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;

  public class Shock : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Shock")
        .ManaCost("{R}")
        .Type("Instant")
        .Text("Shock deals 2 damage to target creature or player.")
        .Cast(p =>
          {
            p.Effect = () => new DealDamageToTargets(2);
            p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());
            p.TargetingRule(new DealDamage(2));
            p.TimingRule(new TargetRemoval());
          });
    }
  }
}