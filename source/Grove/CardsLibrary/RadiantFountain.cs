namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class RadiantFountain : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Radiant Fountain")
        .Type("Land")
        .Text("When Radiant Fountain enters the battlefield, you gain 2 life.{EOL}{T}: Add {1} to your mana pool.")
        .FlavorText("\"All peoples treasure a place where the weary traveler may drink in peace.\"—Ajani Goldmane")
        .ManaAbility(p =>
        {
          p.Text = "{T}: Add {1} to your mana pool.";
          p.ManaAmount(1.Colorless());
        })
        .TriggeredAbility(p =>
        {
          p.Text = "When Radiant Fountain enters the battlefield, you gain 2 life.";
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
          p.Effect = () => new ChangeLife(amount: 2, yours: true);
        });
    }
  }
}
