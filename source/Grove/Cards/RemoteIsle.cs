namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Mana;

  public class RemoteIsle : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Remote Isle")
        .Type("Land")
        .Text(
          "Remote Isle enters the battlefield tapped.{EOL}{T}: Add {U} to your mana pool.{EOL}{Cycling} {2}({2}, Discard this card: Draw a card.)")
        .Timing(Timings.Lands())
        .Cycling("{2}")
        .Abilities(
          ManaAbility(new ManaUnit(ManaColors.Blue), "{T}: Add {U} to your mana pool."))
        .Effect<PutIntoPlay>(e => e.PutIntoPlayTapped = true);
    }
  }
}