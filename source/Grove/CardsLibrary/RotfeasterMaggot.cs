namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;
  using Triggers;

  public class RotfeasterMaggot : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Rotfeaster Maggot")
        .ManaCost("{4}{B}")
        .Type("Creature — Insect")
        .Text("When Rotfeaster Maggot enters the battlefield, exile target creature card from a graveyard. You gain life equal to that card's toughness.")
        .FlavorText("Is it at the top of the food chain or the bottom?")
        .Power(3)
        .Toughness(5)        
        .TriggeredAbility(p =>
        {
          p.Text = "When Rotfeaster Maggot enters the battlefield, exile target creature card from a graveyard. You gain life equal to that card's toughness.";
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

          p.Effect = () => new CompoundEffect(
              new ExileTargets(),
              new ChangeLife(
                amount: P(e => e.Target.Card().Toughness.GetValueOrDefault(), EvaluateAt.AfterTriggeredAbilityTargets),
                whos: P(e => e.Controller)));                    

          p.TargetSelector.AddEffect(trg => trg.Is.Creature().In.Graveyard());

          p.TargetingRule(new EffectOrCostRankBy(c => -c.Toughness.GetValueOrDefault()));
        });
    }
  }
}
