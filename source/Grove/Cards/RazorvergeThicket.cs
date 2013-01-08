namespace Grove.Cards
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Mana;

  public class RazorvergeThicket : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Razorverge Thicket")
        .Type("Land")
        .Text(
          "Razorverge Thicket enters the battlefield tapped unless you control two or fewer other lands.{EOL}{T}: Add {G} or {W} to your mana pool.")
        .FlavorText(
          "Where the Razor Fields beat back the Tangle, the crowded thicket yields to bright scimitars of grass.")
        .Cast(p => p.Effect = Effect<PutIntoPlay>(e => e.PutIntoPlayTapped = e.Controller.Battlefield.Lands.Count() > 2))
        .Abilities(
          ManaAbility(new ManaUnit(ManaColors.Green | ManaColors.White), "{T}: Add {G} or {W} to your mana pool."));
    }
  }
}