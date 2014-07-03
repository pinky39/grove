namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using Effects;
  using Triggers;

  public class IridescentDrake : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Iridescent Drake")
        .ManaCost("{3}{U}")
        .Type("Creature Drake")
        .Text(
          "{Flying}{EOL}When Iridescent Drake enters the battlefield, put target Aura card from a graveyard onto the battlefield under your control attached to Iridescent Drake.")
        .OverrideScore(p => p.Battlefield = Scores.ManaCostToScore[3])
        .Power(2)
        .Toughness(2)
        .SimpleAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Iridescent Drake enters the battlefield, put target Aura card from a graveyard onto the battlefield under your control attached to Iridescent Drake.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new EnchantOwnerWithTarget();
            
            p.TargetSelector.AddEffect(trg => trg.Is.Card(p1 => p1.Target.Card().Is().Aura &&
              p1.Target.Card().CanTarget(p1.OwningCard)).In.Graveyard());
            
            p.TargetingRule(new EffectAttachToOwningCard());
          }
        );
    }
  }
}