namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;

  public class PollutedMire : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Polluted Mire")
        .Type("Land")
        .Text(
          "Polluted Mire enters the battlefield tapped.{EOL}{T}: Add {B} to your mana pool.{EOL}{Cycling} {2}({2}, Discard this card: Draw a card.)")
        .Cast(p => Effect<PutIntoPlay>(e => e.PutIntoPlayTapped = true))
        .Cycling("{2}")
        .Abilities(
          ManaAbility(new ManaUnit(ManaColors.Black), "{T}: Add {B} to your mana pool."));
    }
  }
}