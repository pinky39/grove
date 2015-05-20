namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Triggers;

  public class MonkIdealist : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Monk Idealist")
        .ManaCost("{2}{W}")
        .Type("Creature - Human Monk Cleric")
        .Text(
          "When Monk Idealist enters the battlefield, return target enchantment card from your graveyard to your hand.")
        .FlavorText("Belief is the strongest mortar.")
        .OverrideScore(p => p.Battlefield = Scores.ManaCostToScore[2])
        .Power(2)
        .Toughness(2)
        .Cast(p =>
          {
            p.TimingRule(new OnFirstMain());
            p.TimingRule(new WhenYourGraveyardCountIs(minCount: 1, selector: c => c.Is().Enchantment));
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Monk Idealist enters the battlefield, return target enchantment card from your graveyard to your hand.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new ReturnToHand();
            p.TargetSelector.AddEffect(
              trg => trg.Is.Enchantment().In.YourGraveyard(),
              trg => trg.Message = "Select an enchantment in your graveyard.");

            p.TargetingRule(new EffectOrCostRankBy(c => -c.Score));
          });
    }
  }
}