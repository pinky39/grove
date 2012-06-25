namespace Grove.Cards
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;

  public class CopperlineGorge : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Copperline Gorge")
        .Type("Land")
        .Text(
          "Copperline Gorge enters the battlefield tapped unless you control two or fewer other lands.{EOL}{T}: Add {R} or {G} to your mana pool.")
        .Timing(Timings.Lands())
        .Abilities(
          C.ManaAbility(
            new Mana(ManaColors.Red | ManaColors.Green),
            "{T}: Add {R} or {G} to your mana pool."))
        .Effect<PutIntoPlay>((e, c) =>
          e.PutIntoPlayTapped = player => player.Battlefield.Lands.Count() > 2);
    }
  }
}