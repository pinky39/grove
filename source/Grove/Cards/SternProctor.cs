namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Triggers;
  using Gameplay.Zones;

  public class SternProctor : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Stern Proctor")
        .ManaCost("{U}{U}")
        .Type("Creature Human Wizard")
        .Text(
          "When Stern Proctor enters the battlefield, return target artifact or enchantment to its owner's hand.")
        .FlavorText(
          "I preferred the harsh tutors—they made mischief all the more fun.")
        .Power(1)
        .Toughness(2)
        .Cast(p =>
          {
            p.TimingRule(new OnFirstMain());
            p.TimingRule(new WhenOpponentControllsPermanents(c => c.Is().Artifact || c.Is().Enchantment));
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Stern Proctor enters the battlefield, return target artifact or enchantment to its owner's hand.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new ReturnToHand {Category = EffectCategories.Bounce};
            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Artifact || c.Is().Enchantment).On.Battlefield());
            p.TargetingRule(new EffectBounce());
          });
    }
  }
}