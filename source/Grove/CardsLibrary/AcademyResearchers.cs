namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;
  using Triggers;

  public class AcademyResearchers : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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
            p.Effect = () => new AttachTargetToOwningCard();
            p.TargetSelector.AddEffect(trg =>
              {
                trg.Is.Card(p1 => p1.Target.Card().Is().Aura &&
                  p1.Target.Card().CanTarget(p1.OwningCard)).In.OwnersHand();

                trg.MinCount = 0;
                trg.MaxCount = 1;
              });
            p.TargetingRule(new EffectAttachToOwningCard());
          }
        );
    }
  }
}