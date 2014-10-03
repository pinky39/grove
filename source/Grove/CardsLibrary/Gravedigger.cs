namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Triggers;

  public class Gravedigger : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Gravedigger")
        .ManaCost("{3}{B}")
        .Type("Creature — Zombie")
        .Text(
          "When Gravedigger enters the battlefield, you may return target creature card from your graveyard to your hand.")
        .FlavorText("A grave is not always for burial.")
        .Power(2)
        .Toughness(2)
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Gravedigger enters the battlefield, you may return target creature card from your graveyard to your hand.";

            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

            p.Effect = () => new ReturnToHand();

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().In.YourGraveyard());

            p.TargetingRule(new EffectOrCostRankBy(c => -c.Score));
            p.TimingRule(new WhenYourGraveyardCountIs(c => c.Is().Creature));
          });
    }
  }
}