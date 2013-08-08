namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.Triggers;
  using Gameplay.Zones;

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
            p.TimingRule(new Lands());
            p.TimingRule(new ControllerHasMana(1));
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