namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Triggers;

  public class DarkHatchling : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Dark Hatchling")
        .ManaCost("{4}{B}{B}")
        .Type("Creature Horror")
        .Text(
          "{Flying}{EOL}When Dark Hatchling enters the battlefield, destroy target nonblack creature. It can't be regenerated.")
        .Power(3)
        .Toughness(3)
        .Cast(p => p.TimingRule(new WhenOpponentControllsPermanents(
          card => card.Is().Creature &&
            !card.HasColor(CardColor.Black) &&
              !card.HasProtectionFrom(CardColor.Black), minCount: 1))
        )
        .SimpleAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Dark Hatchling enters the battlefield, destroy target nonblack creature. It can't be regenerated.";

            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new DestroyTargetPermanents(canRegenerate: false);

            p.TargetSelector.AddEffect(trg => trg
              .Is.Card(c => c.Is().Creature && !c.HasColor(CardColor.Black))
              .On.Battlefield());

            p.TargetingRule(new EffectDestroy());
          });
    }
  }
}