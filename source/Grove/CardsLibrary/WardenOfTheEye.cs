namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;
  using Triggers;

  public class WardenOfTheEye : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Warden of the Eye")
        .ManaCost("{2}{U}{R}{W}")
        .Type("Creature — Djinn Wizard")
        .Text("When Warden of the Eye enters the battlefield, return target noncreature, nonland card from your graveyard to your hand.")
        .FlavorText("The wardens guard the sacred documents of Tarkir's history, though they are forbidden to read the words.")
        .Power(3)
        .Toughness(3)
        .TriggeredAbility(p =>
        {
          p.Text = "When Warden of the Eye enters the battlefield, return target noncreature, nonland card from your graveyard to your hand.";
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

          p.Effect = () => new ReturnToHand();

          p.TargetSelector.AddEffect(trg => trg.Is.Card(c => !c.Is().Creature && !c.Is().Land).On.YourGraveyard());
          p.TargetingRule(new EffectOrCostRankBy(c => -c.Score));
        });
    }
  }
}
