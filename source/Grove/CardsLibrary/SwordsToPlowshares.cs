namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;

  public class SwordsToPlowshares : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Swords to Plowshares")
        .ManaCost("{W}")
        .Type("Instant")
        .Text("Exile target creature. Its controller gains life equal to its power.")
        .Cast(p =>
          {
            p.Effect = () => new CompoundEffect(
              new ExileTargets(),
              new ChangeLife(
                amount: P(e => e.Target.Card().Power.GetValueOrDefault()),
                whos: P(e => e.Target.Card().Controller)));

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            
            p.TargetingRule(new EffectExileBattlefield());
            p.TimingRule(new TargetRemovalTimingRule().RemovalTags(EffectTag.Exile, EffectTag.CreaturesOnly));
          });
    }
  }
}