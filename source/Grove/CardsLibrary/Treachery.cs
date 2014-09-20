namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;
  using Triggers;

  public class Treachery : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Treachery")
        .ManaCost("{3}{U}{U}")
        .Type("Enchantment Aura")
        .Text("When Treachery enters the battlefield, untap up to five lands.{EOL}You control enchanted creature.")
        .FlavorText("The academy educates; I employ. It's a perfect arrangement.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(() => new ChangeController(m => m.SourceCard.Controller))
              .SetTags(EffectTag.ChangeController);

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectGainControl());
          })
        .TriggeredAbility(p =>
          {
            p.Text = "When Treachery enters the battlefield, untap up to five lands.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new UntapSelectedPermanents(
              minCount: 0,
              maxCount: 5,
              validator: c => c.Is().Land,
              text: "Select lands to untap."
              );
          }
        );
    }
  }
}