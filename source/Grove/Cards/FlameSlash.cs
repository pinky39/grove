namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;

  public class FlameSlash : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Flame Slash")
        .ManaCost("{R}")
        .Type("Sorcery")
        .Text("Flame Slash deals 4 damage to target creature.")
        .FlavorText(
          "After millennia asleep, the Eldrazi had forgotten about Zendikar's fiery temper and dislike of strangers.")
        .Cast(p =>
          {
            p.Effect = () => new DealDamageToTargets(4);
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new DealDamage(4));
            p.TimingRule(new TargetRemoval());
          });
    }
  }
}