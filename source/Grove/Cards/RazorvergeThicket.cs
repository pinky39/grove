namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;


  public class RazorvergeThicket : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Razorverge Thicket")
        .Type("Land")
        .Text(
          "Razorverge Thicket enters the battlefield tapped unless you control two or fewer other lands.{EOL}{T}: Add {G} or {W} to your mana pool.")
        .FlavorText(
          "Where the Razor Fields beat back the Tangle, the crowded thicket yields to bright scimitars of grass.")
        .Timing(Timings.Lands())
        .Abilities(
          C.ManaAbility(new Mana(ManaColors.Green | ManaColors.White), "{T}: Add {G} or {W} to your mana pool."))
        .Effect<PutIntoPlay>((e, _) => e.PutIntoPlayTapped = player => player.Battlefield.Lands.Count() > 2);
    }
  }
}