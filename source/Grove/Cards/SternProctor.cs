namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Ai.TargetingRules;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;
  using Core.Triggers;
  using Core.Zones;

  public class SternProctor : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
            p.TimingRule(new FirstMain());
            p.TimingRule(new OpponentHasPermanents(c => c.Is().Artifact || c.Is().Enchantment));
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Stern Proctor enters the battlefield, return target artifact or enchantment to its owner's hand.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new ReturnToHand {Category = EffectCategories.Bounce};
            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Artifact || c.Is().Enchantment).On.Battlefield());
            p.TargetingRule(new Bounce());
          });
    }
  }
}