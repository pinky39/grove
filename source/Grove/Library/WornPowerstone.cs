namespace Grove.Library
{
  using System.Collections.Generic;
  using Grove.Gameplay;
  using Grove.Gameplay.Effects;

  public class WornPowerstone : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Worn Powerstone")
        .ManaCost("{3}")
        .Type("Artifact")
        .Text("Worn Powerstone enters the battlefield tapped.{EOL}{T}: Add {2} to your mana pool.")
        .Cast(p => p.Effect = () => new PutIntoPlay(tap: true))
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {2} to your mana pool.";
            p.ManaAmount(2.Colorless());
          });
    }
  }
}