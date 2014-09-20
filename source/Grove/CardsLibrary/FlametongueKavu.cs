namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Triggers;

  public class FlametongueKavu : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Flametongue Kavu")
        .ManaCost("{3}{R}")
        .Type("Creature Kavu")
        .Text("When Flametongue Kavu enters the battlefield, it deals 4 damage to target creature.")
        .FlavorText("For dim-witted, thick-skulled genetic mutants, they have pretty good aim.")
        .OverrideScore(p => p.Battlefield = Scores.ManaCostToScore[3])
        .Power(4)
        .Toughness(2)
        .Cast(p => p.TimingRule(new WhenOpponentControllsPermanents(
          card => card.Is().Creature && card.Life <= 4 &&
            card.CanBeTargetBySpellsWithColor(CardColor.Red))))
        .TriggeredAbility(p =>
          {
            p.Text = "When Flametongue Kavu enters the battlefield, it deals 4 damage to target creature.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new DealDamageToTargets(4);
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new EffectDealDamage(4));
          });
    }
  }
}