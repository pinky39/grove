namespace Grove.Cards
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;

  public class RazorvergeThicket : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Razorverge Thicket")
        .Type("Land")
        .Text(
          "Razorverge Thicket enters the battlefield tapped unless you control two or fewer other lands.{EOL}{T}: Add {G} or {W} to your mana pool.")
        .FlavorText(
          "Where the Razor Fields beat back the Tangle, the crowded thicket yields to bright scimitars of grass.")
        .Cast(p => p.Effect = () => new PutIntoPlay(tapIf: e => e.Controller.Battlefield.Lands.Count() > 2))
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {G} or {W} to your mana pool.";
            p.ManaAmount(new ManaUnit(ManaColors.Green | ManaColors.White));
          });
    }
  }
}