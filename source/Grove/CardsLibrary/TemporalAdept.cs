namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;

  public class TemporalAdept : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Temporal Adept")
        .ManaCost("{1}{U}{U}")
        .Type("Creature Human Wizard")
        .Text("{U}{U}{U},{T}: Return target permanent to its owner's hand.")
        .FlavorText("Of course she's at the head of her class. All her classmates have disappeared.")
        .Power(1)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text = "{U}{U}{U},{T}: Return target permanent to its owner's hand.";
            p.Cost = new AggregateCost(
              new PayMana("{U}{U}{U}".Parse(), ManaUsage.Abilities),
              new Tap());

            p.Effect = () => new Effects.ReturnToHand();
            p.TargetSelector.AddEffect(trg => trg.Is.Card().On.Battlefield());
            p.TargetingRule(new EffectBounce());
            p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.Bounce));
          });
    }
  }
}