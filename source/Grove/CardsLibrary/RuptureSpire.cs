namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TimingRules;
  using Grove.Triggers;

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
            p.Effect = () => new CastPermanent(tap: true);
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
            p.Effect = () => new PayManaThen(1.Colorless(),
              effect: new SacrificeOwner(),
              parameters: new PayThen.Parameters()
              {
                ExecuteIfPaid = false,
              });
          });
    }
  }
}