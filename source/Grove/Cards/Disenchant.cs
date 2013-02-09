namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TargetingRules;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;

  public class Disenchant : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Disenchant")
        .ManaCost("{1}{W}")
        .Type("Instant")
        .Text("Destroy target artifact or enchantment.")
        .FlavorText("'Let Phyrexia breed evil in the darkness; my holy light will reveal its taint.{EOL}—Serra")
        .Cast(p =>
          {
            p.Effect = () => new DestroyTargetPermanents();
            p.TargetSelector.AddEffect(trg => trg
              .Is.Card(card => card.Is().Artifact || card.Is().Enchantment)
              .On.Battlefield());

            p.TargetingRule(new OrderByRank(c => -c.Score, ControlledBy.Opponent));
            p.TimingRule(new TargetRemoval());
          });
    }
  }
}