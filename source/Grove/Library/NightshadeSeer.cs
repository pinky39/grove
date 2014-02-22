namespace Grove.Library
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay.AI;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;

  public class NightshadeSeer : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Nightshade Seer")
        .ManaCost("{3}{B}")
        .Type("Creature Human Wizard")
        .Text(
          "{2}{B},{T}:Reveal any number of black cards in your hand. Target creature gets -X/-X until end of turn, where X is the number of cards revealed this way.")
        .Power(1)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text =
              "{2}{B},{T}:Reveal any number of black cards in your hand. Target creature gets -X/-X until end of turn, where X is the number of cards revealed this way.";

            p.Cost = new AggregateCost(
              new PayMana("{2}{B}".Parse(), ManaUsage.Abilities),
              new Tap());

            p.Effect = () => new CreatureGetsM1M1ForEachRevealedCard();

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            
            p.TargetingRule(new EffectReduceToughness(
              getAmount: tp => tp.Controller.Hand.Count(c => c.HasColor(CardColor.Black))));

            p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.ReduceToughness));
          });
    }
  }
}