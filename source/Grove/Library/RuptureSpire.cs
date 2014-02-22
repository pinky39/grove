namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Triggers;

  public class RuptureSpire : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Rupture Spire")
        .Type("Land")
        .Text(
          "Rupture Spire enters the battlefield tapped.{EOL}When Rupture Spire enters a battlefield, sacrifice it unless you pay {1}.{EOL}{T}: Add one mana of any color to your mana pool.")
        .Cast(p =>
          {
            p.TimingRule(new DefaultLandsTimingRule());
            p.TimingRule(new WhenYouHaveMana(1));
            p.Effect = () => new PutIntoPlay(tap: true);
          })
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add one mana of any color to your mana pool.";
            p.ManaAmount(new SingleColorManaAmount(ManaColor.Any, 1));
          })
        .TriggeredAbility(p =>
          {
            p.Text = "When Rupture Spire enters the battlefield, sacrifice it unless you pay {1}.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new PayManaOrSacrifice(1.Colorless());
          });
    }
  }
}