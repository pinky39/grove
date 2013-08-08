namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Triggers;
  using Gameplay.Zones;

  public class MonkRealist : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Monk Realist")
        .ManaCost("{1}{W}")
        .Type("Creature Human Monk Cleric")
        .Text("When Monk Realist enters the battlefield, destroy target enchantment.")
        .FlavorText("We plant the seeds of doubt to harvest the crop of wisdom.")
        .Power(1)
        .Toughness(1)
        .Cast(p =>
          {
            p.TimingRule(new FirstMain());
            p.TimingRule(new OpponentHasPermanents(c => c.Is().Enchantment));
          })
        .TriggeredAbility(p =>
          {
            p.Text = "When Monk Realist enters the battlefield, destroy target enchantment.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new DestroyTargetPermanents();
            p.TargetSelector.AddEffect(trg => trg.Is.Enchantment().On.Battlefield());

            p.TargetingRule(new Destroy());
          }
        );
    }
  }
}