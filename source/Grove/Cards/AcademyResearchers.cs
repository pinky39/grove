namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TargetingRules;
  using Core.Dsl;
  using Core.Effects;
  using Core.Targeting;
  using Core.Triggers;
  using Core.Zones;

  public class AcademyResearchers : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Academy Researchers")
        .ManaCost("{1}{U}{U}")
        .Type("Creature Human Wizard")
        .Text(
          "When Academy Researchers enters the battlefield, you may put an Aura card from your hand onto the battlefield attached to Academy Researchers.")
        .Power(2)
        .Toughness(2)
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Academy Researchers enters the battlefield, you may put an Aura card from your hand onto the battlefield attached to Academy Researchers.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new EnchantOwnerWithTarget();
            p.TargetSelector.AddEffect(ps => ps
              .Is.Card(p1 =>
                p1.Target.Card().Is().Aura &&
                  p1.Target.Card().CanTarget(p1.Effect.Source.OwningCard))
              .In.OwnersHand());
            p.TargetingRule(new AttachTargetToSelf());
          }
        );
    }
  }
}