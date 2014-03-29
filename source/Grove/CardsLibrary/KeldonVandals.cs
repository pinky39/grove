namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Triggers;

  public class KeldonVandals : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Keldon Vandals")
        .ManaCost("{2}{R}")
        .Type("Creature Human Rogue")
        .Text("{Echo} {2}{R}{EOL}When Keldon Vandals enters the battlefield, destroy target artifact.")
        .FlavorText("Keldons divide all their spoils into two groups: trophies and catapult ammunition.")
        .Power(4)
        .Toughness(1)
        .Echo("{2}{R}")
        .Cast(p =>
          {
            p.TimingRule(new WhenOpponentControllsPermanents(c => c.Is().Artifact));
            p.TimingRule(new OnFirstMain());
          })
        .TriggeredAbility(p =>
          {
            p.Text = "When Keldon Vandals enters the battlefield, destroy target artifact.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new DestroyTargetPermanents();
            p.TargetSelector.AddEffect(trg => trg
              .Is.Card(card => card.Is().Artifact)
              .On.Battlefield());
            p.TargetingRule(new EffectRankBy(c => -c.Score, ControlledBy.Opponent));
          }
        );
        
    }
  }
}