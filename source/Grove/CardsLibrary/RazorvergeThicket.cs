namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Effects;

  public class RazorvergeThicket : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Razorverge Thicket")
        .Type("Land")
        .Text(
          "Razorverge Thicket enters the battlefield tapped unless you control two or fewer other lands.{EOL}{T}: Add {G} or {W} to your mana pool.")
        .FlavorText(
          "Where the Razor Fields beat back the Tangle, the crowded thicket yields to bright scimitars of grass.")
        .Cast(p => p.Effect = () => new PutIntoPlay(tap: P(e => e.Controller.Battlefield.Lands.Count() > 2)))
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {G} or {W} to your mana pool.";
            p.ManaAmount(Mana.Colored(isGreen: true, isWhite: true));
          });
    }
  }
}