namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;
  using Triggers;

  public class SageEyeAvengers : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sage-Eye Avengers")
        .ManaCost("{4}{U}{U}")
        .Type("Creature — Djinn Monk")
        .Text("{Prowess}{I}(Whenever you cast a noncreature spell, this creature gets +1/+1 until end of turn.){/I}{EOL}Whenever Sage-Eye Avengers attacks, you may return target creature to its owner's hand if its power is less than Sage-Eye Avengers's power.")
        .Power(4)
        .Toughness(5)
        .Prowess()
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever Sage-Eye Avengers attacks, you may return target creature to its owner's hand if its power is less than Sage-Eye Avengers's power.";
          p.Trigger(new WhenThisAttacks());
          p.Effect = () => new ReturnToHand();
          p.TargetSelector.AddEffect(trg => trg.Is.Card(x => x.Power < P(e => e.Source.OwningCard.Power)).On.Battlefield());
          p.TargetingRule(new EffectOrCostRankBy(c => -c.Score * c.Power.GetValueOrDefault(1)));
        });
    }
  }
}
