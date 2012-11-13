namespace Grove.Cards
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Mana;

  public class CopperlineGorge : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Copperline Gorge")
        .Type("Land")
        .Text(
          "Copperline Gorge enters the battlefield tapped unless you control two or fewer other lands.{EOL}{T}: Add {R} or {G} to your mana pool.")
        .Timing(Timings.Lands())
        .Abilities(
          ManaAbility(
            new ManaUnit(ManaColors.Red | ManaColors.Green),
            "{T}: Add {R} or {G} to your mana pool."))
        .Effect<PutIntoPlay>(e =>
          e.PutIntoPlayTapped = e.Controller.Battlefield.Lands.Count() > 2);
    }
  }
}