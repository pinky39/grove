namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Triggers;

  public class MonkRealist : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Monk Realist")
        .ManaCost("{1}{W}")
        .Type("Creature Human Monk Cleric")
        .Text("When Monk Realist enters the battlefield, destroy target enchantment.")
        .FlavorText("We plant the seeds of doubt to harvest the crop of wisdom.")
        .OverrideScore(p => p.Battlefield = Scores.ManaCostToScore[1])
        .Power(1)
        .Toughness(1)
        .Cast(p =>
          {
            p.TimingRule(new OnFirstMain());
            p.TimingRule(new WhenOpponentControllsPermanents(c => c.Is().Enchantment));
          })
        .TriggeredAbility(p =>
          {
            p.Text = "When Monk Realist enters the battlefield, destroy target enchantment.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new DestroyTargetPermanents();
            p.TargetSelector.AddEffect(trg => trg.Is.Enchantment().On.Battlefield());

            p.TargetingRule(new EffectDestroy());
          }
        );
    }
  }
}